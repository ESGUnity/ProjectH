using System.Collections.Generic;
using UnityEngine;

public class CardObjPool : MonoBehaviour
{
    // 상수
    private const int initialSize = 60;

    // 프리팹
    [Header("프리팹")]
    [SerializeField] private GameObject prefab_CardObj;

    // private 필드
    private Queue<GameObject> pool = new Queue<GameObject>();

    // 싱글턴
    private static CardObjPool instance;
    public static CardObjPool Instance => instance;

    // 유니티 콜백
    private void Awake()
    {
        // 싱글턴
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializePool();
    }

    // 메인
    private void InitializePool()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject go = CreateNewObject();
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }
    private GameObject CreateNewObject()
    {
        GameObject go = Instantiate(prefab_CardObj, transform);
        return go;
    }
    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            GameObject go = pool.Dequeue();
            go.SetActive(true);
            return go;
        }

        // 풀에 없으면 새로 생성
        return CreateNewObject();
    }
    public void ReturnObject(GameObject go)
    {
        go.SetActive(false);
        pool.Enqueue(go);
    }
}
