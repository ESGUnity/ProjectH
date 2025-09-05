using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public static class UtilityAndHelper
{
    static System.Random rng = new System.Random(); // 난수 생성기

    public static List<T> Shuffle<T>(IList<T> list)
    {
        // 리스트 복사
        List<T> copy = new List<T>(list);

        // Fisher-Yates Shuffle
        int n = copy.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T value = copy[k];
            copy[k] = copy[n];
            copy[n] = value;
        }

        return copy;
    }

    static readonly List<RaycastResult> uiRaycastResults = new();
    public static bool IsPointerEnterObj(GameObject target) // UI와 WorldObj 모두 포인터 엔터인지 반환
    {
        if (target == null)
        {
            return false;
        }

        // UI 체크 (RectTransform 여부로 판단)
        if (IsUI(target))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            uiRaycastResults.Clear();
            EventSystem.current.RaycastAll(pointerData, uiRaycastResults);

            foreach (var result in uiRaycastResults)
            {
                if (result.gameObject == target || result.gameObject.transform.IsChildOf(target.transform))
                    return true;
            }
        }
        // 3D 체크 (Collider 여부로 판단)
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.gameObject == target || hit.collider.transform.IsChildOf(target.transform))
                    return true;
            }
        }

        return false;
    }
    static bool IsUI(GameObject go)
    {
        return go.GetComponent<RectTransform>() != null;
    }
}

// 구조체
public struct PRS
{
    public Vector3 Pos;
    public Vector3 Rot;
    public Vector3 Scale;
}

// 열거형
public enum GameStateEnum
{
    None, Setting, Round,
}
public enum InitGameEnum
{
    None, NewGame, LoadGame,
}
public enum CardRarityEnum
{
    Common, Rare, Epic
}