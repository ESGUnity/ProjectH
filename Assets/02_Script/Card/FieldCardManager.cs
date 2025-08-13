using System.Collections.Generic;
using UnityEngine;

public class FieldCardManager : MonoBehaviour
{
    [Header("컴포넌트")]
    PlayerCard pCard;

    FieldCardManager fieldCardManager;
    HandCardManager handCardManager;
    AcquiredCardManager acquiredCardManager;

    [Header("프리팹")]
    [SerializeField] GameObject prefab_CardObj;

    [Header("주요 프로퍼티")]
    Vector3 MIDDLE_PILE_POS = new Vector3(0, 0, 0);

    [Header("싱글턴")]
    static FieldCardManager instance;
    public static FieldCardManager Instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        pCard = GetComponent<PlayerCard>();

        fieldCardManager = GetComponent<FieldCardManager>();
        handCardManager = GetComponent<HandCardManager>();
        acquiredCardManager = GetComponent<AcquiredCardManager>();
    }

    public void GenerateMiddlePile() // 중간 더미 생성
    {
        // 덱 셔플 후 캐싱
        List<Card> cards = UtilityAndHelper.Shuffle(pCard.GetPlayerOwnedCards());

        // 덱을 통해서 오브젝트 생성
        foreach (Card card in cards)
        {
            GameObject go = Instantiate(prefab_CardObj);
            go.GetComponent<CardObj>().SetCardInfo(card);
        }


    }
}
