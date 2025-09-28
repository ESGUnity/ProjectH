using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    // private 필드
    private List<CardInstance> playerHandCards;
    private List<CardInstance> oppoHandCards;

    // public Getter
    public List<CardInstance> PlayerHandCards { get; private set; }
    public List<CardInstance> OppoHandCards { get; private set; }

    // 유니티 콜백
}
