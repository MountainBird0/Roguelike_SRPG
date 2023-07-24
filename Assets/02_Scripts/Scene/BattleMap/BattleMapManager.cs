/**********************************************************
* 배틀 맵 관리
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BattleMapManager : MonoBehaviour
{
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

    public static BattleMapManager instance;

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


        CreateTile();
    }

    /**********************************************************
    * 타일 dic 생성
    ***********************************************************/
    private void CreateTile()
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


    public void test()
    {
        foreach (KeyValuePair<Vector3Int, TileLogic> pair in deployTiles)
        {
            Vector3Int key = pair.Key;
            TileLogic value = pair.Value;

            // 키와 값에 대한 작업 수행
            Debug.Log($"{GetType()} Key: " + key + ", Value: " + value);
            Debug.Log($"{GetType()} content: " + value.content);
        }
    }
}
