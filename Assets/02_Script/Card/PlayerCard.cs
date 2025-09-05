using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    // private 필드
    private List<CardInstance> deckCards;
    private List<CardInstance> handCards;

    // public Getter
    public List<CardInstance> OriginDeckCards => deckCards;
    public List<CardInstance> CloneDeckCards => deckCards.ToList();
}