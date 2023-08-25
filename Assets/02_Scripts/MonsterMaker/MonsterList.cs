/**********************************************************
* 각 맵의 몬스터의 종류와 좌표를 지정할 ScriptableObject
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterMaker", menuName = "Scriptable Object/MonsterMaker", order = 0)]
public class MonsterList : ScriptableObject
{
    public List<MonsterInfo> monsters;
}

