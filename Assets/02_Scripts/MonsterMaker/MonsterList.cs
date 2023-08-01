using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterMaker", menuName = "Scriptable Object/MonsterMaker", order = 0)]
public class MonsterList : ScriptableObject
{
    public List<MonsterInfo> monsters;
}

