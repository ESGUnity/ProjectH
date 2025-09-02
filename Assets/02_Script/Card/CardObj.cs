using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// 패 오브젝트에 붙을 컴포넌트
public class CardObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 상수
    private const float POINTER_ENTER_SCALE_AMOUNT = 1.25f;
    private const float POINTER_ENTER_ANIMATION_TIME = 0.15f;

    // private 필드(컴포넌트)
    [SerializeField] private Collider col;
    [SerializeField] private SpriteRenderer cardBaseSprite;

    // private 필드
    private Card cardInfo;
    private PRS originPRS;
    private int OriginOrder;
    private List<GameStateEnum> VALID_STATES;
    private GameStateEnum prevState;

    // 유니티 콜백
    private void Awake()
    {
        // 컴포넌트 할당
        TryGetComponent(out col);
        TryGetComponent(out cardBaseSprite);

        // 변수 할당
        VALID_STATES = new();
    }
    private void Update()
    {
        // 캐싱
        GameStateEnum currentState = GameFlowManager.Instance.GameState;

        if (prevState != currentState) // GameFlowState가 바뀐 경우
        {
            if (!GameFlowManager.Instance.IsInState(VALID_STATES)) // 유효한 상태가 아닌 경우
            {
                ForceExit();
            }

            if (!VALID_STATES.Contains(prevState) && VALID_STATES.Contains(currentState)) // VALID_STATE로 변한 시점에 포인터 엔터가 되어있다면 Enter 발동
            {
                if (UtilityAndHelper.IsPointerEnterObj(col.gameObject))
                {
                    ForceEnter();
                }
            }

            prevState = currentState;
        }
    }
    private void OnDisable()
    {
        ForceExit();
    }
    public void SetCardInfo(Card card)
    {
        // 안전장치
        if (card == null) 
        {
            Debug.LogError("CardObj Send : Card 클래스가 null입니다.");
            return;
        }

        cardInfo = card; // 카드 정보 할당
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ForceEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ForceExit();
    }
    public void ForceEnter()
    {
        if (GameFlowManager.Instance.IsInState(VALID_STATES))
        {
            //transform.DOKill();
            //transform.DOKill();

            //SetOrder(1000);
            //transform.DOScale(originPRS.Scale * POINTER_ENTER_SCALE_AMOUNT, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
            //transform.DOLocalRotate(Vector3.zero, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);

            // TODO : 사운드
            //AudioManager.Instance.PlaySFX(SFXEnum.CardHovering);
        }
    }
    public void ForceExit()
    {
        //if (!isEnter) return;

        //transform.DOKill();

        //SetOrder(OriginOrder);
        //transform.DOScale(originPRS.Scale, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
        //transform.DOLocalRotate(originPRS.Rot, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
    }




    //void OnCardBaseLoadComplete(AsyncOperationHandle<Sprite> opHandle)
    //{
    //    if (opHandle.Status == AsyncOperationStatus.Succeeded)
    //    {
    //        sprite_CardBase.sprite = opHandle.Result;
    //    }
    //}
    //public void SetOrder(int order) // 카드 요소의 소팅 오더를 정렬
    //{
    //    // 카드 베이스
    //    sprite_CardBase.sortingLayerName = "WorldObject";
    //    sprite_CardBase.sortingOrder = order + 1;
    //    sprite_CardFrame.sortingLayerName = "WorldObject";
    //    sprite_CardFrame.sortingOrder = order + 5;
    //    text_CardType.GetComponent<MeshRenderer>().sortingLayerName = "WorldObject";
    //    text_CardType.GetComponent<MeshRenderer>().sortingOrder = order + 6;
    //    text_CardNumber.GetComponent<MeshRenderer>().sortingLayerName = "WorldObject";
    //    text_CardNumber.GetComponent<MeshRenderer>().sortingOrder = order + 6;

    //    sprite_CardEffect.sortingLayerName = "WorldObject";
    //    sprite_CardEffect.sortingOrder = order + 2;
    //    sprite_Engraving.sortingLayerName = "WorldObject";
    //    sprite_Engraving.sortingOrder = order + 3;
    //    sprite_CursedEffect.sortingLayerName = "WorldObject";
    //    sprite_CursedEffect.sortingOrder = order + 7;
    //    sprite_CursedEffect2.sortingLayerName = "WorldObject";
    //    sprite_CursedEffect2.sortingOrder = order + 7;
    //}
}
