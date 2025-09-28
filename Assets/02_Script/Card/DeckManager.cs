using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    // private 필드
    private List<CardInstance> originDeckCards;

    // public Getter
    public List<CardInstance> OriginDeckCards => originDeckCards;
    public List<CardInstance> CloneDeckCards => originDeckCards.ToList();

    private void Awake()
    {
        InitDeckCards();
    }
    private void InitDeckCards()
    {
        originDeckCards = UtilityAndHelper.Shuffle(JsonLoader.LoadInitCards());
    }
}