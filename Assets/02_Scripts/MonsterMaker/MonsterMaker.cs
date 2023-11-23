/**********************************************************
* monsterList�� ������� ���� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaker : MonoBehaviour
{
    public MonsterList monsterList;

    /**********************************************************
    * ���� ����
    ***********************************************************/
    public void CreateMonters(Dictionary<Vector3Int, TileLogic> mainTiles)
    {
        for (int i = 0; i < monsterList.monsters.Count; i++)
        {
            var info = monsterList.monsters[i];
            GameObject ob = ObjectPoolManager.instance.Spawn(info.name);
            ob.transform.position = info.pos;
            mainTiles[info.pos].content = ob;

            var unit = ob.GetComponent<Unit>();
            unit.currentPos = info.pos;
            BattleMapManager.instance.UnitSetting(unit);
        }
    }
}
