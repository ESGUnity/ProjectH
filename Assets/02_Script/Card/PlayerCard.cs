using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    // private 필드
    private List<Card> deckCards;
    private List<Card> handCards;

    // public Getter
    public List<Card> OriginDeckCards => deckCards;
    public List<Card> CloneDeckCards => deckCards.ToList();
}