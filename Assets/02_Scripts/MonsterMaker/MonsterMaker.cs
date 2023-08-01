using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMaker : MonoBehaviour
{
    public MonsterList monsterList;

    public void Start()
    {
        CreateMonters();
    }

    private void CreateMonters()
    {
        for (int i = 0; i < monsterList.monsters.Count; i++)
        {
            var info = monsterList.monsters[i];
            GameObject ob = ObjectPoolManager.instance.Spawn(info.name);
            ob.transform.position = info.pos;

            // content¿¡ ³Ö±â
            //mainTiles[info.pos].content = ob;
        }
    }
}
