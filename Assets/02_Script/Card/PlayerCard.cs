using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    // 필드
    private List<Card> deckCards;
    public List<Card> OriginDeckCards => deckCards;
    public List<Card> CloneDeckCards => deckCards.ToList();
    private List<Card> handCards;
}
