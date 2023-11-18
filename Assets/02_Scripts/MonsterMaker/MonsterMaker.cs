/**********************************************************
* monsterList의 정보대로 몬스터 생성
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaker : MonoBehaviour
{
    public MonsterList monsterList;

    /**********************************************************
    * 몬스터 생성
    ***********************************************************/
    public void CreateMonters(Dictionary<Vector3Int, TileLogic> mainTiles)
    {
        for (int i = 0; i < monsterList.monsters.Count; i++)
        {
            var info = monsterList.monsters[i];
            GameObject ob = ObjectPoolManager.instance.Spawn(info.name);
            ob.transform.position = info.pos;
            mainTiles[info.pos].content = ob;

            BattleMapManager.instance.UnitSetting(ob.GetComponent<Unit>());
        }
    }
}
