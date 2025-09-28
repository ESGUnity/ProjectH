using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public static class UtilityAndHelper
{
    // private 필드
    private static System.Random rng = new System.Random(); // 난수 생성기
    private static readonly List<RaycastResult> uiRaycastResults = new(); // UI 판별기 변수

    // public 필드
    public static float AlignementCardDuration = 0.3f;

    // 메인
    public static List<T> Shuffle<T>(IList<T> list) // 리스트를 섞은 후 리스트의 복제본을 반환
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
    public static bool IsPointerEnterObj(GameObject target) // UI와 WorldObj 모두 포인터 핸들러를 사용 가능한 상태인지 반환
    {
        if (target == null)
        {
            return false;
        }

        if (IsUI(target)) // UI 체크 (RectTransform 여부로 판단)
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
        else // 3D 체크 (Collider 여부로 판단)
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
    private static bool IsUI(GameObject go)
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

    public PRS(Vector3 pos, Vector3 rot, Vector3 scale)
    {
        Pos = pos;
        Rot = rot;
        Scale = scale;
    }
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
[Flags]
public enum CardTypeEnum
{
    None,
    Gwang,
    Kkeut_Bird, Kkeut_Boar, Kkeut_Deer, // 4, 3, 3
    Tti_ChungDan, Tti_HongDan, Tti_ChoDan, // 3, 3, 4
    SsangPi,
}