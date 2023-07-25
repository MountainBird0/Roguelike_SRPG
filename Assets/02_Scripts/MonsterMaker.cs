using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterMaker", menuName = "Scriptable Object/MonsterMaker", order = 0)]
public class MonsterMaker : ScriptableObject
{
    public List<MonsterInfo> monsters;
}

[System.Serializable]
public class MonsterInfo
{
    public string name;
    public Vector3Int pos;
}
