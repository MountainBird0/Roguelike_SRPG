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
    * Ÿ�ϵ� �ҷ���
    * ref ���� �׳� �ٷ� �߰��ϰ� �����
    ***********************************************************/
    public void GetTiles(List<Vector3Int> maps, List<Vector3Int> highlights, List<Vector3Int> deploySpots)
    {
        LoadTiles(map, maps);
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

}
