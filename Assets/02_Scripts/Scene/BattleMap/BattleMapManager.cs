/**********************************************************
* ��Ʋ �� ����
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


    #region �� ����
    [HideInInspector]
    public Board board; // ������ �ʿ��� �ҷ��� board

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
    * �ʷε�
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap();
        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();

        // Ÿ�ϸ� ����
        board.GetTiles(mainMaps, highlights, deploySpots);

        //foreach (var t in deploySpots)
        //{
        //    Debug.Log($"{GetType()} - Ÿ���̸� ��ġ - {t}, {t.x}, {t.y}");
        //}


        CreateTile();
    }

    /**********************************************************
    * Ÿ�� dic ����
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

            // Ű�� ���� ���� �۾� ����
            Debug.Log($"{GetType()} Key: " + key + ", Value: " + value);
            Debug.Log($"{GetType()} content: " + value.content);
        }
    }
}
