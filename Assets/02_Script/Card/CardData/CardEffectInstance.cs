using System;
using System.Collections.Generic;

[Serializable]
public class CardEffectInstance
{
    public string Key { get; }
    public string Name { get; }
    public string Description { get; }
    public CardRarityEnum Rarity { get; }
    public Dictionary<string, float> Params { get; set; }


    public CardEffectInstance(string key, string name, string desc, CardRarityEnum rarity, Dictionary<string, float> param)
    {
        Key = key;
        Name = name;
        Description = desc;
        Rarity = rarity;
        Params = new Dictionary<string, float>(param); // 런타임용 복사
    }
}
