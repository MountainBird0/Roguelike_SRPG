using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap map;
    public Tilemap highlight;
    public Tilemap deploySpot;

    /**********************************************************
    * 타일들 불러옴
    * ref 말고 그냥 바로 추가하게 만들기
    ***********************************************************/
    public void GetTiles(List<Vector3Int> maps, List<Vector3Int> highlights, List<Vector3Int> deploySpots)
    {
        LoadTiles(map, maps);
        LoadTiles(highlight, highlights);
        LoadTiles(deploySpot, deploySpots);
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

}
