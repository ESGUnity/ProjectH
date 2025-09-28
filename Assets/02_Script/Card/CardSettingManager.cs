using DG.Tweening;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

// 중간 더미 및 바닥 패 세팅
public class CardSettingManager : MonoBehaviour
{
    // 상수
    private const int MAX_CARD_COUNT = 10;
    private const float CARD_Z_VALUE = -0.01f;
    private readonly Vector3 CARD_ORIGIN_SCALE = new Vector3(1f, 1f, 1f);
    private readonly Vector3 HAND_START_POS = new Vector3(-10f, 0, 0);
    private readonly Vector3 HAND_END_POS = new Vector3(10f, 0, 0);
    private const float HAND_FULL_LENGTH = 20f;
    private readonly Vector3 MIDDLE_PILE_POS = new Vector3(0, 0, 0);

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private Transform middlePile;
    [SerializeField] private Transform playerHand;
    [SerializeField] private Transform oppoHand;
    [SerializeField] private Transform floor;

    // private 필드(컴포넌트)
    private DeckManager deckManager;
    private HandManager handManager;
    private CardSettingManager cardSettingManager;

    // private 필드
    private List<GameObject> middlePileCards = new(); // 라운드 진행 시 관리할 카드 오브젝트 리스트
    private List<GameObject> playerHandCards = new();
    private List<GameObject> oppoHandCards = new();
    private List<GameObject> floorCards = new();

    // 싱글턴
    private static CardSettingManager instance;
    public static CardSettingManager Instance {  get { return instance; } }

    // 유니티 콜백
    private void Awake()
    {
        // 싱글턴
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // 컴포넌트 할당
        TryGetComponent(out deckManager);
        TryGetComponent(out handManager);
        TryGetComponent(out cardSettingManager);
    }

    // 메인
    public void SetMiddlePile() // 라운드 시작 시 게임에서 사용될 패 오브젝트를 생성(중간 더미)
    {
        List<CardInstance> cards = UtilityAndHelper.Shuffle(deckManager.CloneDeckCards); // 덱 셔플 후 캐싱

        // 라운드 시작 전 리스트 초기화
        middlePileCards.Clear();

        // 플레이어 덱으로 라운드에 사용될 카드 오브젝트 생성
        foreach (CardInstance card in cards)
        {
            GameObject go = CardObjPool.Instance.GetObject(); // 풀에서 카드 오브젝트 가져오기
            middlePileCards.Add(go); // 라운드에서 관리할 카드 오브젝트 리스트에 추가
            go.transform.SetParent(middlePile); // 부모 지정
            go.transform.localPosition = Vector3.zero;
            go.transform.localEulerAngles = Vector3.zero;
            go.GetComponentInChildren<CardObj>().SetCardInfo(card); // 오브젝트 내에 CardObj에서 카드 정보 설정하기
            go.GetComponentInChildren<CardObjSpriteLoader>().FlipToVerso();
            go.SetActive(false);
        }
    }
    public async Task DealCards() // 손 패 및 바닥 패 나누기
    {
        // 맞고 규칙대로 4 / 4 / 2로 손 패 나누기
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                await FlipCardOnMiddlePile(playerHandCards, playerHand);
            }
            for (int k = 0; k < 4; k++)
            {
                await FlipCardOnMiddlePile(oppoHandCards, oppoHand);
            }
        }
        for (int i = 0; i < 2; i++)
        {
            await FlipCardOnMiddlePile(playerHandCards, playerHand);
        }
        for (int j = 0; j < 2; j++)
        {
            await FlipCardOnMiddlePile(oppoHandCards, oppoHand);
        }

        // 바닥 패 세팅
        for (int k = 0; k < 8; k++)
        {
            await FlipCardOnMiddlePile(floorCards, floor);
        }
    }
    private async Task FlipCardOnMiddlePile(List<GameObject> target, Transform parent) // 중간 더미 뒤집기 
    {
        // 패 활성화
        GameObject selectedMiddlePile = middlePileCards[middlePileCards.Count - 1];
        selectedMiddlePile.SetActive(true);
        selectedMiddlePile.GetComponentInChildren<CardObjSpriteLoader>().FlipToFront();

        // 부모 지정 및 리스트 관리
        selectedMiddlePile.transform.SetParent(parent);
        target.Add(selectedMiddlePile);
        middlePileCards.RemoveAt(middlePileCards.Count - 1);

        // 손패 정렬
        await Task.WhenAll
            (
                AlignmentHandCards(playerHandCards),
                AlignmentHandCards(oppoHandCards)
            );
    }
    private async Task AlignmentHandCards(List<GameObject> targetCards) // 손 패 정렬
    {
        List<PRS> cardPRS = SetHandCardsPos(targetCards.Count);
        List<Task> tweenTask = new();

        // TODO : 추후 카드가 정렬될 순서 로직 추가

        // 카드의 OriginPRS 지정
        for (int i = 0; i < targetCards.Count; i++)
        {
            // 카드 위치 설정
            targetCards[i].GetComponentInChildren<CardObj>().OriginPRS = cardPRS[i];

            // 소팅오더 설정
            targetCards[i].GetComponentInChildren<CardObjSpriteLoader>().OriginOrder = (i + 1) * 10;
            targetCards[i].GetComponentInChildren<CardObjSpriteLoader>().SetOrder((i + 1) * 10);

            // 두트윈
            Task move = targetCards[i].transform
                .DOLocalMove(targetCards[i].GetComponentInChildren<CardObj>().OriginPRS.Pos, UtilityAndHelper.AlignementCardDuration)
                .SetEase(Ease.OutQuart)
                .AsyncWaitForCompletion();
            Task rotate = targetCards[i].transform
                .DOLocalRotate(targetCards[i].GetComponentInChildren<CardObj>().OriginPRS.Rot, UtilityAndHelper.AlignementCardDuration)
                .SetEase(Ease.OutQuart)
                .AsyncWaitForCompletion();
            Task scale = targetCards[i].transform
                .DOLocalRotate(targetCards[i].GetComponentInChildren<CardObj>().OriginPRS.Scale, UtilityAndHelper.AlignementCardDuration)
                .SetEase(Ease.OutQuart)
                .AsyncWaitForCompletion();
            tweenTask.Add(Task.WhenAll(move, rotate, scale));

        }

        await Task.WhenAll(tweenTask);
    }
    private List<PRS> SetHandCardsPos(int cardCount) // 손 패 위치 설정
    {
        List<PRS> results = new List<PRS>(cardCount);
        float interval;

        if (cardCount == 0) return results;

        // 카드 위치 간격 설정
        if (cardCount == 1)
        {
            interval = 0.5f;
        }
        else
        {
            interval = 1f / (cardCount - 1); 
        }

        float usedLength = HAND_FULL_LENGTH * cardCount / MAX_CARD_COUNT;

        Vector3 center = (HAND_START_POS + HAND_END_POS) / 2f;
        Vector3 dir = (HAND_START_POS - HAND_END_POS).normalized;
        Vector3 start = center - dir * (usedLength / 2f);
        Vector3 end = center + dir * (usedLength / 2f);


        for (int i = 0; i < cardCount; i++)
        {
            Vector3 pos = Vector3.Lerp(start, end, interval * i);
            pos = new Vector3(pos.x, pos.y, i * CARD_Z_VALUE);

            Vector3 rot = new Vector3(0, 0, 0);
            Vector3 scale = CARD_ORIGIN_SCALE;

            results.Add(new PRS(pos, rot, scale));
        }

        return results;
    }
}
