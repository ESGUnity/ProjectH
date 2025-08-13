using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    public List<Card> OwnedCards { get; private set; }


    public List<Card> GetPlayerOwnedCards()
    {
        return OwnedCards.ToList();
    }
}
