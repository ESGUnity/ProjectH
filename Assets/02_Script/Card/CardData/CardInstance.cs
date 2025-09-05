using System;
using System.Collections.Generic;

[Serializable]
public class CardInstance
{
    public int Month;
    public bool IsGwang;
    public bool IsGgeut;
    public bool IsTti;
    public bool IsSsangPi;

    public CardEffectInstance Effect;

    public CardInstance(int month, bool isGwang, bool isGgeut, bool isTti, bool isSsangPi)
    {
        Month = month;
        IsGwang = isGwang;
        IsGgeut = isGgeut;
        IsTti = isTti;
        IsSsangPi = isSsangPi;
    }
}
