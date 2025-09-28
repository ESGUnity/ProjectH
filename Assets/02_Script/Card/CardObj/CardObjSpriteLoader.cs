using System.Net;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CardObjSpriteLoader : MonoBehaviour
{
    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_SpriteRenderer;

    // 씬 오브젝트
    [Header("씬 오브젝트")]
    [SerializeField] private SpriteRenderer cardBase;
    [SerializeField] private SpriteRenderer sprite_BrushStroke;
    [SerializeField] private TMP_Text text_Month;
    [SerializeField] private Transform cardTypeSprites;

    // private 필드(컴포넌트)
    private CardObj cardObj;

    // private 필드
    private int remainingToLoad; // 카드 타입 남은 로드 수

    // public 
    public int OriginOrder; // 각 요소의 소팅 오더

    // 유니티 콜백
    private void Awake()
    {
        TryGetComponent(out cardObj);
    }

    // 메인
    public void SetCardObjSprites()
    {
        CardInstance cardInfo = cardObj.CardInfo;

        // 월 텍스트 할당
        text_Month.text = cardInfo.Month.ToString();

        // 붓질 스프라이트 할당
        if (cardInfo.Effect != null)
        {
            string address = $"Sprite_{cardInfo.Effect.Name}";
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
            handle.Completed += OnCompletedBrushStroke;
        }

        // 카드 타입 스프라이트 할당
        if (cardInfo.Types.Count == 0)
        {
            SetOrder(OriginOrder);
            return;
        }

        remainingToLoad = cardInfo.Types.Count;
        foreach (CardTypeEnum type in cardInfo.Types)
        {
            string address = $"Sprite_{type.ToString()}";

            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
            handle.Completed += OnCompletedCardTypeSprite;
        }
    }
    private void OnCompletedBrushStroke(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            sprite_BrushStroke.sprite = obj.Result;
        }
    }
    private void OnCompletedCardTypeSprite(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject go = Instantiate(prefab_SpriteRenderer, cardTypeSprites);
            go.GetComponent<SpriteRenderer>().sprite = obj.Result;
        }

        remainingToLoad--;
        if (remainingToLoad <= 0)
        {
            SetCardTypeSpritesLayout();
            SetOrder(OriginOrder);
        }
    }
    private void SetCardTypeSpritesLayout()
    {
        float spriteSize = 0.5f;
        int perCol = 6;

        Vector3 startPos = new Vector3(1f - spriteSize / 2f, 1.5f - spriteSize / 2f, 0f);

        for (int i = 0; i < cardTypeSprites.childCount; i++)
        {
            int col = i / perCol;
            int row = i % perCol;

            float x = startPos.x - col * spriteSize;
            float y = startPos.y - row * spriteSize;

            cardTypeSprites.GetChild(i).localPosition = new Vector3(x, y, 0f);
        }
    }
    public void SetOrder(int order) // 카드 요소의 소팅 오더를 정렬
    {
        cardBase.sortingLayerName = "Obj";
        cardBase.sortingOrder = order;
        sprite_BrushStroke.sortingLayerName = "Obj";
        sprite_BrushStroke.sortingOrder = order + 1;
        text_Month.GetComponent<MeshRenderer>().sortingLayerName = "Obj";
        text_Month.GetComponent<MeshRenderer>().sortingOrder = order + 3;

        foreach (Transform t in cardTypeSprites)
        {
            t.GetComponent<SpriteRenderer>().sortingLayerName = "Obj";
            t.GetComponent<SpriteRenderer>().sortingOrder = order + 2;
        }
    }
    public void FlipToFront() // 앞면으로 뒤집기
    {
        // 카드 베이스 불러오기
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>("Sprite_CardBase_Front");
        handle.Completed += OnCompletedLoadCardBase;

        // 다른 모든 요소 활성화
        sprite_BrushStroke.gameObject.SetActive(true);
        text_Month.gameObject.SetActive(true);
        foreach (Transform t in cardTypeSprites)
        {
            t.gameObject.SetActive(true);
        }
    }
    public void FlipToVerso() // 뒷면으로 뒤집기
    {
        // 카드 베이스 불러오기
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>("Sprite_CardBase_Verso");
        handle.Completed += OnCompletedLoadCardBase;

        // 다른 모든 요소 활성화
        sprite_BrushStroke.gameObject.SetActive(false);
        text_Month.gameObject.SetActive(false);
        foreach (Transform t in cardTypeSprites)
        {
            t.gameObject.SetActive(false);
        }
    }
    private void OnCompletedLoadCardBase(AsyncOperationHandle<Sprite> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            cardBase.sprite = obj.Result;
        }
    }
}
