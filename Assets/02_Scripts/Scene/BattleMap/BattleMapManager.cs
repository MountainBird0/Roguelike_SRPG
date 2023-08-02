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

    ///**********************************************************
    //* Ÿ�� ������ �´� Ÿ�� ����Ʈ�� ��ȯ
    //***********************************************************/
    //public List<TileLogic> Search(TileLogic start, Func<TileLogic, TileLogic, bool> searchType)
    //{
    //    List<TileLogic> tilesResult = new List<TileLogic>();

    //    tilesResult.Add(start);
    //    ClearSearch();

    //    Queue<TileLogic> checkNext = new Queue<TileLogic>();
    //    Queue<TileLogic> checkNow = new Queue<TileLogic>();

    //    start.distance = 0;
    //    checkNow.Enqueue(start);

    //    while (checkNow.Count > 0)
    //    {
    //        TileLogic t = checkNow.Dequeue();
    //        for (int i = 0; i < 4; i++)
    //        {
    //            TileLogic next = GetTile(t.pos + dirs[i]);

    //            if (next == null || next.distance <= t.distance + 1)
    //            {

    //                continue;
    //            }
    //            if(searchType(t, next))
    //            {
    //                next.prev = t;
    //                checkNext.Enqueue(next);
    //                tilesResult.Add(next);
    //            }
    //        }

    //        if(checkNow.Count == 0)
    //        {
    //            SwapReference(ref checkNow, ref checkNext);
    //        }
    //    }
    //    return tilesResult;
    //}


    //private void SwapReference(ref Queue<TileLogic> now, ref Queue<TileLogic> next)
    //{
    //    Queue<TileLogic> temp = now;
    //    now = next;
    //    next = temp;
    //}
    //public TileLogic GetTile(Vector3Int pos)
    //{
    //    TileLogic tile = null;
    //    mainTiles.TryGetValue(pos, out tile);

    //    return tile;
    //}
    //void ClearSearch()
    //{
    //    foreach (TileLogic t in mainTiles.Values)
    //    {
    //        t.prev = null;
    //        t.distance = int.MaxValue;
    //    }
    //}
    //public static TileLogic GetTile(Vector3Int pos) // �ٸ��������� �� �� �ְ� �ϱ� ����
    //{
    //    TileLogic tile = null;
    //    instance.tiles.TryGetValue(pos, out tile);

    //    return tile;
    //}

    /**********************************************************
    * ���� ����Ʈ�� ���� �ֱ�
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        unit.tile = TL;
    }


    /**********************************************************
    * ���� ����Ʈ�� Player�ִ��� Ȯ��
    ***********************************************************/
    public bool IsHuman()
    {
        for(int i = 0; i < units.Count; i++)
        {
            if(units[i].playerType == PlayerType.Human)
            {
                return true;
            }
        }
        return false;
    }
}
