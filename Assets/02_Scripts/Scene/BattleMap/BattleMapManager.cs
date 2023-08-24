/**********************************************************
* 배틀 맵 관리
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
    public Board board; // 생성된 맵에서 불러올 board

    public List<Unit> units; // 유닛 리스트

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
    * 맵로딩
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
    }


    /**********************************************************
    * 유닛 리스트에 유닛 넣기
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        unit.tile = TL;
        if(DataManager.instance.currentUnitStats.ContainsKey(unit.unitName))
        {
            unit.stats = DataManager.instance.currentUnitStats[unit.unitName];
        }
        // 몬스터도 나중에 추가
    }


    /**********************************************************
    * 유닛 리스트에 Player있는지 확인
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
