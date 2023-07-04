/******************************************************************************
* 각 풀에 대한 정보를 하나의 클래스로 묶어 정의
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KeyType = System.String;

[System.Serializable]
public class PoolObjectData
{
    public const int INITIAL_COUNT = 10;
    public const int MAX_COUNT = 50;

    public KeyType key;
    public GameObject prefab;
    public int initialObjectCount = 0; // 오브젝트 초기 생성 개수
    public int maxObjectCount = 0;     // 큐 내에 보관할 수 있는 오브젝트 최대 개수
}
