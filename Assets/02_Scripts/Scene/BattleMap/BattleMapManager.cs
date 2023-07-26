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
    public Grid grid;

    public Dictionary<Vector3Int, TileLogic> mainTiles;
    public Dictionary<Vector3Int, TileLogic> deployTiles;

    #region 맵 관련
    [HideInInspector]
    public Board board; // 생성된 맵에서 불러올 board
    # endregion
     
    public List<Vector3Int> mainMaps;
    public List<Vector3Int> highlights;
    public List<Vector3Int> deploySpots;

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

        mainTiles = new Dictionary<Vector3Int, TileLogic>();
        deployTiles = new Dictionary<Vector3Int, TileLogic>();

        mainMaps = new List<Vector3Int>();
        highlights = new List<Vector3Int>();
        deploySpots = new List<Vector3Int>();
    }

    /**********************************************************
    * 맵로딩
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap();
        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();

        // 타일맵 세팅
        board.GetTiles(mainMaps, highlights, deploySpots);

        //foreach (var t in deploySpots)
        //{
        //    Debug.Log($"{GetType()} - 타일이름 위치 - {t}, {t.x}, {t.y}");
        //}


        CreateTiles();
        CreateMonters();
    }

    /**********************************************************
    * 타일 dic 생성
    ***********************************************************/
    private void CreateTiles()
    {
        for(int i = 0; i < mainMaps.Count; i++)
        {
            if (!mainTiles.ContainsKey(mainMaps[i]))
            {
                Vector3 worldPos = grid.CellToWorld(mainMaps[i]);
                TileLogic tileLogic = new TileLogic(mainMaps[i], worldPos);
                mainTiles.Add(mainMaps[i], tileLogic);
            }
        }

        for (int i = 0; i < deploySpots.Count; i++)
        {
            if (!deployTiles.ContainsKey(mainMaps[i]))
            {
                Vector3 worldPos = grid.CellToWorld(deploySpots[i]);
                TileLogic tileLogic = new TileLogic(deploySpots[i], worldPos);
                deployTiles.Add(deploySpots[i], tileLogic);
            }
        }
    }

    /**********************************************************
    * 몬스터 생성
    ***********************************************************/
    private void CreateMonters()
    {
        for(int i = 0; i < board.monsterMaker.monsters.Count; i++)
        {
            var info = board.monsterMaker.monsters[i];
            GameObject ob = ObjectPoolManager.instance.Spawn(info.name);
            ob.transform.position = info.pos;

            mainTiles[info.pos].content = ob;
        }
    }


    /**********************************************************
    * deployTiles타일의 content를 mainTiles로 복사
    ***********************************************************/
    public void CopyContent()
    {
        foreach(var pair in deployTiles)
        {
            if(pair.Value.content != null)
            {
                mainTiles[pair.Key].content = pair.Value.content;
            }
        }
    }

    /**********************************************************
    * units에 unit추가
    ***********************************************************/
    public void AddUnit()
    {
        foreach(var pair in mainTiles)
        {
            if(pair.Value.content != null && pair.Value.content.CompareTag("Unit"))
            {
                units.Add(pair.Value.content.GetComponent<Unit>());
            }
        }
    }



    public void test()
    {
        CopyContent();
        AddUnit();

        //foreach (KeyValuePair<Vector3Int, TileLogic> pair in deployTiles)
        //{
        //    Vector3Int key = pair.Key;
        //    TileLogic value = pair.Value;

        //    Debug.Log($"{GetType()} Key: " + key + ", Value: " + value);
        //    Debug.Log($"{GetType()} content: " + value.content);
        //}

        //foreach (var pair in mainTiles)
        //{
        //    Vector3Int key = pair.Key;
        //    TileLogic value = pair.Value;

        //    Debug.Log($"{GetType()} main Key: " + key + ", Value: " + value);
        //    Debug.Log($"{GetType()} main content: " + value.content);
        //}

        //foreach(var unit in units)
        //{
        //    Debug.Log($"{GetType()} - 유닛체크 - {unit} -- {unit.unitName}");
        //}

        // ObjectPoolManager.instance.Despawn(units[0].gameObject);


    }


    /**********************************************************
    * 타일 범위에 맞는 타일 리스트를 반환
    ***********************************************************/
    public List<TileLogic> Search(TileLogic start, Func<TileLogic, TileLogic, bool> searchType)
    {
        List<TileLogic> tilesResult = new List<TileLogic>();

        tilesResult.Add(start);
        ClearSearch();

        Queue<TileLogic> checkNext = new Queue<TileLogic>();
        Queue<TileLogic> checkNow = new Queue<TileLogic>();

        start.distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            TileLogic t = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                TileLogic next = GetTile(t.pos + dirs[i]);

                if (next == null || next.distance <= t.distance + 1)
                {

                    continue;
                }
                if(searchType(t, next))
                {
                    next.prev = t;
                    checkNext.Enqueue(next);
                    tilesResult.Add(next);
                }
            }

            if(checkNow.Count == 0)
            {
                SwapReference(ref checkNow, ref checkNext);
            }
        }
        return tilesResult;
    }


    private void SwapReference(ref Queue<TileLogic> now, ref Queue<TileLogic> next)
    {
        Queue<TileLogic> temp = now;
        now = next;
        next = temp;
    }
    public TileLogic GetTile(Vector3Int pos)
    {
        TileLogic tile = null;
        mainTiles.TryGetValue(pos, out tile);

        return tile;
    }
    void ClearSearch()
    {
        foreach (TileLogic t in mainTiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }
    //public static TileLogic GetTile(Vector3Int pos) // 다른곳에서도 쓸 수 있게 하기 위해
    //{
    //    TileLogic tile = null;
    //    instance.tiles.TryGetValue(pos, out tile);

    //    return tile;
    //}
}
