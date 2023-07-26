using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap mainMap;
    public Tilemap highlight;
    public Tilemap deploySpot;

    public MonsterMaker monsterMaker;

    public Tile blueHighlightTile;

    /**********************************************************
    * Ÿ�ϵ� �ҷ���
    ***********************************************************/
    public void GetTiles(List<Vector3Int> maps, List<Vector3Int> highlights, List<Vector3Int> deploySpots)
    {
        LoadTiles(mainMap, maps);
        LoadTiles(highlight, highlights);
        LoadTiles(deploySpot, deploySpots);
    }

    /**********************************************************
    * Ÿ�ϸ� ���� Ÿ�ϵ��� ��ġ�� List<Vector3Int>�� ��ȯ
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
    * ���̶���Ʈ Ÿ�Ϸ� ����
    ***********************************************************/
    public void SetHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlight.SetTile(tiles[i].pos, blueHighlightTile);
        }
    }

    /**********************************************************
    * ���̶���Ʈ Ÿ�� �����
    ***********************************************************/
    public void ClearHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlight.SetTile(tiles[i].pos, null);
        }
    }
}
