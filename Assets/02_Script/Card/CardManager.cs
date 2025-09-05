using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private List<CardInstance> cardList;

    private void Awake()
    {
        LoadCards();
        LogCardMonths();
    }

    // JSON에서 카드 불러오기
    private void LoadCards()
    {
        if (!File.Exists(JsonLoader.BASIC_CARD_PATH)) // 예외 처리
        {
            Debug.LogError($"Card JSON file not found at: {JsonLoader.BASIC_CARD_PATH}");
            cardList = new List<CardInstance>();
            return;
        }

        string json = File.ReadAllText(JsonLoader.BASIC_CARD_PATH);
        cardList = JsonConvert.DeserializeObject<List<CardInstance>>(json); // 역직렬화
    }

    // 각 카드의 Month를 로그로 출력
    private void LogCardMonths()
    {
        foreach (var card in cardList)
        {
            Debug.Log($"Card Month: {card.Month}");
        }
    }

    // 외부에서 초기 카드 리스트 접근 가능
    public List<CardInstance> GetInitCards()
    {
        return cardList;
    }
}
