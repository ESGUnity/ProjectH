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
    private CardObjSpriteLoader spriteLoader;
    private Collider col;
    private SpriteRenderer cardBaseSprite;

    // private 필드
    private readonly List<GameStateEnum> VALID_STATES = new();
    private CardInstance cardInfo;
    private GameStateEnum prevState;

    // public 필드
    public PRS OriginPRS;

    // pulblic Setter
    public CardInstance CardInfo { get { return cardInfo; } }

    // 유니티 콜백
    private void Awake()
    {
        // 컴포넌트 할당
        TryGetComponent(out spriteLoader);
        TryGetComponent(out col);
        TryGetComponent(out cardBaseSprite);
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

    // 메인
    public void SetCardInfo(CardInstance card)
    {
        cardInfo = card; // 카드 정보 할당
        spriteLoader.SetCardObjSprites(); // 스프라이트 세팅
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


}
