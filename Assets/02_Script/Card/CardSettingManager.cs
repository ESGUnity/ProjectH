using System.Collections.Generic;
using UnityEngine;

// 중간 더미 및 바닥 패 세팅
public class CardSettingManager : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_CardObj;

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private GameObject obj_MiddlePile;

    // 컴포넌트
    private PlayerCard pCard;
    private OppoCard oCard;
    private CardSettingManager cardSettingManager;

    // 필드
    private readonly Vector3 MIDDLE_PILE_POS = new Vector3(0, 0, 0);
    private List<GameObject> cardObjs = new();

    // 싱글턴
    private static CardSettingManager instance;
    public static CardSettingManager Instance;

    void Awake()
    {
        // 싱글턴
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 컴포넌트 할당
        TryGetComponent(out pCard);
        TryGetComponent(out oCard);
        TryGetComponent(out cardSettingManager);
    }

    public void GenerateInitCards() // 판 시작 시 게임에서 사용될 패 오브젝트를 생성(중간 더미)
    {
        List<Card> cards = UtilityAndHelper.Shuffle(pCard.CloneDeckCards); // 덱 셔플 후 캐싱

        // 해당 판에 사용할 패 오브젝트 생성
        foreach (Card card in cards)
        {
            GameObject go = CardObjPool.Instance.GetObject();
            go.GetComponent<CardObj>().SetCardInfo(card);
        }
    }
    public void DealHandCards() // 플레이어와 상대의 손 패를 나누기
    {
        List<Card> cards = UtilityAndHelper.Shuffle(pCard.CloneDeckCards); // 덱 셔플 후 캐싱



    }
}
