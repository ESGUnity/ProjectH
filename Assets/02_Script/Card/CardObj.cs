using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("주요 프로퍼티")]
    [HideInInspector] public Card CardInfo;
    [HideInInspector] public PRS OriginPRS;
    [HideInInspector] public int OriginOrder;

    [Header("씬 오브젝트")]
    Collider cardCollider;
    SpriteRenderer sprite_CardBase;

    [Header("포인터 프로퍼티")]
    bool isEnter = false;
    protected List<GameFlowStateEnum> VALID_STATES;
    GameFlowStateEnum prevState;
    const float POINTER_ENTER_SCALE_AMOUNT = 1.25f;

    void Awake()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        cardCollider = Array.Find(transforms, c => c.gameObject.name.Equals("Sprite_CardBase")).GetComponent<Collider>();
        sprite_CardBase = Array.Find(transforms, c => c.gameObject.name.Equals("Sprite_CardBase")).GetComponent<SpriteRenderer>();

        VALID_STATES = new();
    }
    void Update()
    {
        // 캐싱
        GameFlowStateEnum currentState = GameFlowManager.Instance.GameFlowState;

        if (prevState != currentState) // GameFlowState가 바뀐 경우
        {
            if (!GameFlowManager.Instance.IsInState(VALID_STATES)) // 유효한 상태가 아닌 경우
            {
                ForceExit();
            }
            else if (isEnter && !GameFlowManager.Instance.IsInState(VALID_STATES))
            {
                ForceExit();
            }
            else if (!VALID_STATES.Contains(prevState) && VALID_STATES.Contains(currentState)) // VALID_STATE로 변한 시점에 포인터 엔터가 되어있다면 Enter 발동
            {
                if (UtilityAndHelper.IsPointerEnterObj(cardCollider.gameObject))
                {
                    ForceEnter();
                }
            }

            prevState = currentState;
        }
    }
    void OnDisable()
    {
        ForceExit();
    }
    public void SetCardInfo(Card card)
    {

    }
    #region 포인터
    public void OnPointerEnter(PointerEventData eventData)
    {
        ForceEnter();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ForceExit();
    }
    public virtual void ForceEnter()
    {
        if (GameFlowManager.Instance.IsInState(VALID_STATES))
        {
            transform.DOKill();
            obj_Card.transform.DOKill();

            SetOrder(1000);
            transform.DOScale(OriginPRS.Scale * POINTER_ENTER_SCALE_AMOUNT, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
            transform.DOLocalRotate(Vector3.zero, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
            obj_Card.transform.DOLocalRotate(Vector3.zero, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);

            S_HoverInfoSystem.Instance.ActivateHoverInfoByCard(CardInfo, sprite_CardBase.gameObject);

            // 사운드
            S_AudioManager.Instance.PlaySFX(SFXEnum.CardHovering);

            isEnter = true;
        }
    }
    public virtual void ForceExit()
    {
        if (!isEnter) return;

        transform.DOKill();
        obj_Card.transform.DOKill();

        SetOrder(OriginOrder);
        transform.DOScale(OriginPRS.Scale, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);
        transform.DOLocalRotate(OriginPRS.Rot, POINTER_ENTER_ANIMATION_TIME).SetEase(Ease.OutQuart);

        S_HoverInfoSystem.Instance.DeactiveHoverInfo();

        isEnter = false;
    }
    #endregion
}
