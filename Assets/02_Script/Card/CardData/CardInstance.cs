using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable] // Json용
public class CardInstance
{
    public int Month { get; private set; }
    public HashSet<CardTypeEnum> Types { get; set; }
    public CardEffectInstance Effect { get; set; }

    public CardInstance(int month, HashSet<CardTypeEnum> types)
    {
        Month = month;
        Types = new HashSet<CardTypeEnum>(types);
    }
}
