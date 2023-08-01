using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Grid grid;

    [Header("Tile")]
    public Tile blueHighlightTile;

    [Header("TileMap")]
    public Tilemap mainMap;
    public Tilemap highlightMap;
    public Tilemap deployMap;

    public Dictionary<Vector3Int, TileLogic> mainTiles;
    public Dictionary<Vector3Int, TileLogic> deployTiles;

    [Header("Maker")]
    public MonsterMaker monsterMaker;

    private void Awake()
    {
        SetTiles();
    }


    /**********************************************************
    * 타일들 불러옴
    ***********************************************************/
    public void SetTilePos(List<Vector3Int> maps, List<Vector3Int> highlights, List<Vector3Int> deploySpots)
    {
        LoadTiles(mainMap, maps);
        LoadTiles(highlightMap, highlights);
        LoadTiles(deployMap, deploySpots);
    }

    /**********************************************************
    * 타일맵 안의 타일들의 위치를 List<Vector3Int>로 반환
    ***********************************************************/
    private void LoadTiles(Tilemap tilemap, List<Vector3Int> tiles)
    {
        Vector3Int currentPos = new Vector3Int();
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                currentPos.x = x;
                currentPos.y = y;

                if (tilemap.HasTile(currentPos))
                {
                    tiles.Add(currentPos);
                }
            }
        }
    }

    /**********************************************************
    * 타일 Dictionary 생성
    ***********************************************************/
    private void SetTiles()
    {
        SetTile(mainMap, mainTiles);
        SetTile(deployMap, deployTiles);
        
    }
    private void SetTile(Tilemap map, Dictionary<Vector3Int, TileLogic> tiles)
    {
        Vector3Int currentPos = new Vector3Int();
        BoundsInt bounds = map.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                currentPos.x = x;
                currentPos.y = y;

                if (map.HasTile(currentPos))
                {
                    Vector3 worldPos = grid.CellToWorld(currentPos);
                    TileLogic tileLogic = new TileLogic(currentPos, worldPos);
                    tiles.Add(currentPos, tileLogic);
                }
            }
        }
    }


    /**********************************************************
    * 하이라이트 타일 생성 / 지우기
    ***********************************************************/
    public void SetHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlightMap.SetTile(tiles[i].pos, blueHighlightTile);
        }
    }
    public void ClearHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlightMap.SetTile(tiles[i].pos, null);
        }
    }
}
