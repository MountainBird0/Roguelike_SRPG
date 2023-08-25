/******************************************************************************
* 오브젝트풀 관리
*******************************************************************************/
#if UNITY_EDITOR
#define TEST_ON
#endif

using System.Collections.Generic;
using UnityEngine;
using System;

using KeyType = System.String;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private List<PoolObjectData> poolObjectDataList = new List<PoolObjectData>();

    private Dictionary<KeyType, GameObject> sampleDict;   // Key - 복제용 오브젝트 원본
    private Dictionary<KeyType, PoolObjectData> dataDict; // Key - 풀 정보
    private Dictionary<KeyType, Stack<GameObject>> poolDict;         // Key - 풀
    private Dictionary<GameObject, Stack<GameObject>> clonePoolDict; // 복제된 게임오브젝트 - 풀

    public static ObjectPoolManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }

        Init();
    }

    private void Init()
    {
        int len = poolObjectDataList.Count;
        if (len == 0)
        {
            return;
        }

        // 1. Dictionary 생성
        sampleDict = new Dictionary<KeyType, GameObject>(len);
        dataDict = new Dictionary<KeyType, PoolObjectData>(len);
        poolDict = new Dictionary<KeyType, Stack<GameObject>>(len);
        clonePoolDict = new Dictionary<GameObject, Stack<GameObject>>(len * PoolObjectData.INITIAL_COUNT);

        // 2. Data로부터 새로운 Pool 오브젝트 정보 생성
        foreach (var data in poolObjectDataList)
        {
            RegisterInternal(data);
        }
    }

    /// <summary> Pool 데이터로부터 새로운 Pool 오브젝트 정보 등록 </summary>
    private void RegisterInternal(PoolObjectData data)
    {
        // 중복 키는 등록 불가능
        if (poolDict.ContainsKey(data.key))
        {
            return;
        }

        // 1. 샘플 게임오브젝트 생성, PoolObject 컴포넌트 존재 확인
        GameObject sample = Instantiate(data.prefab);
        sample.name = data.prefab.name;
        sample.SetActive(false);

        // 2. Pool Dictionary에 풀 생성 + 풀에 미리 오브젝트들 만들어 담아놓기
        Stack<GameObject> pool = new Stack<GameObject>(data.maxObjectCount);
        for (int i = 0; i < data.initialObjectCount; i++)
        {
            GameObject clone = Instantiate(data.prefab);
            clone.SetActive(false);
            pool.Push(clone);

            clonePoolDict.Add(clone, pool); // Clone-Stack 캐싱
        }

        // 3. 딕셔너리에 추가
        sampleDict.Add(data.key, sample);
        dataDict.Add(data.key, data);
        poolDict.Add(data.key, pool);
    }

    /// <summary> 샘플 오브젝트 복제하기 </summary>
    private GameObject CloneFromSample(KeyType key)
    {
        if (!sampleDict.TryGetValue(key, out GameObject sample)) return null;

        return Instantiate(sample);
    }

    /// <summary> 풀에서 꺼내오기 </summary>
    public GameObject Spawn(KeyType key)
    {
        // 키가 존재하지 않는 경우 null 리턴
        if (!poolDict.TryGetValue(key, out var pool))
        {
            return null;
        }

        GameObject go;

        // 1. 풀에 재고가 있는 경우 : 꺼내오기
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        // 2. 재고가 없는 경우 샘플로부터 복제
        else
        {
            go = CloneFromSample(key);
            clonePoolDict.Add(go, pool); // Clone-Stack 캐싱
        }

        go.SetActive(true);
        go.transform.SetParent(null);

        return go;
    }

    /// <summary> 풀에 집어넣기 </summary>
    public void Despawn(GameObject go)
    {
        // 캐싱된 게임오브젝트가 아닌 경우 파괴
        if (!clonePoolDict.TryGetValue(go, out var pool))
        {
            Destroy(go);
            return;
        }

        // 집어넣기
        go.SetActive(false);
        //go.transform.SetParent(null); // 내가추가
        pool.Push(go);
    }
}
