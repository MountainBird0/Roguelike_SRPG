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

            Debug.Log($"{GetType()} - ��������� {DataManager.instance.defaultMonsterStats[info.name].HP}");
            mainTiles[info.pos].content = ob; // �̰� �Ⱦ�����
            ob.GetComponent<Unit>().stats = DataManager.instance.defaultMonsterStats[info.name];
            Debug.Log($"{GetType()} - ��������� {ob.GetComponent<Unit>().stats.HP}");
            BattleMapManager.instance.AddMonster(ob.GetComponent<Unit>(), mainTiles[info.pos]);
        }
    }
}
