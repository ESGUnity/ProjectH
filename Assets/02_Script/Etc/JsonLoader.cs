using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonLoader
{
    // private 필드
    public static readonly string INIT_CARDS_PATH = Path.Combine(Application.streamingAssetsPath, "initCards.json");

    // 메인
    private static IEnumerable<JObject> EnumerateObjects(string jsonPath) // JSON 배열 순회하며 조건에 맞는 JObject를 반환
    {
        using StreamReader sr = new StreamReader(jsonPath);
        using JsonTextReader reader = new JsonTextReader(sr);

        if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
        {
            throw new InvalidDataException("JSON must be a top-level array.");
        }

        while (reader.Read())
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                yield return JObject.Load(reader);
            }
            else if (reader.TokenType == JsonToken.EndArray)
            {
                yield break;
            }
        }
    }
    public static List<CardInstance> LoadInitCards() // 초기 CardInstance를 반환(48장)
    {
        return JsonConvert.DeserializeObject<List<CardInstance>>(File.ReadAllText(INIT_CARDS_PATH));
    }
    public static CardEffectInstance GetEffectByKeyFromJson(string jsonPath, string targetKey) // Key로 CardEffect 찾기
    {
        foreach (var obj in EnumerateObjects(jsonPath))
        {
            string key = (string)obj["Key"];

            if (string.Equals(key, targetKey, StringComparison.Ordinal))
            {
                return ToInstance(obj);
            }
        }
        return null;
    }
    public static CardEffectInstance GetRandomEffectByRarityFromJson(string jsonPath, CardRarityEnum targetRarity, System.Random rng) // 특정 등급에서 랜덤 CardEffect 1개 선택
    {
        var candidates = new List<JObject>();

        foreach (var obj in EnumerateObjects(jsonPath))
        {
            var rarityStr = (string)obj["Rarity"] ?? "Common";
            if (!Enum.TryParse(rarityStr, true, out CardRarityEnum rarity))
                rarity = CardRarityEnum.Common;

            if (rarity == targetRarity)
                candidates.Add(obj);
        }

        if (candidates.Count == 0) return null;

        var chosen = candidates[rng.Next(candidates.Count)];
        return ToInstance(chosen);
    }
    private static CardEffectInstance ToInstance(JObject obj) // // JObject → CardEffectInstance 변환
    {
        var key = (string)obj["Key"];
        var name = (string)obj["Name"];
        var desc = (string)obj["Description"];
        var rarityStr = (string)obj["Rarity"] ?? "Common";
        if (!Enum.TryParse(rarityStr, true, out CardRarityEnum rarity))
            rarity = CardRarityEnum.Common;

        var dict = obj["Params"]?.ToObject<Dictionary<string, float>>() ?? new Dictionary<string, float>();

        return new CardEffectInstance(key, name, desc, rarity, dict);
    }
}
