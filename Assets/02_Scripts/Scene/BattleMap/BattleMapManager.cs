/**********************************************************
* ��Ʋ �� ����
***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BattleMapManager : MonoBehaviour
{
    public static BattleMapManager instance;
    
    public MapSelector selector;
    public Transform map;

    [HideInInspector]
    public Board board; // ������ �ʿ��� �ҷ��� board

    public List<Unit> units; // ���� ����Ʈ

    private Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

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
    }

    /**********************************************************
    * �ʷε�
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
    }


    /**********************************************************
    * ���� ����Ʈ�� ���� �ֱ�
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        unit.tile = TL;
        if(DataManager.instance.currentUnitStats.ContainsKey(unit.unitName))
        {
            unit.stats = DataManager.instance.currentUnitStats[unit.unitName];
        }
        // ���͵� ���߿� �߰�
    }


    /**********************************************************
    * ���� ����Ʈ�� Player�ִ��� Ȯ��
    ***********************************************************/
    public bool IsHuman()
    {
        for(int i = 0; i < units.Count; i++)
        {
            if(units[i].playerType == PlayerType.HUMAN)
            {
                return true;
            }
        }
        return false;
    }
}
