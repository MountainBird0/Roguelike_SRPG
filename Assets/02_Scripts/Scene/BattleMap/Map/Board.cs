using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap hightlight;
    public Tilemap DeploySpot;

    private void Awake()
    {
        
    }

    private void Start()
    {
        LoadTiles();
    }

    public List<Vector3Int> LoadTiles()
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        Vector3Int currentPos = new Vector3Int();
        BoundsInt bounds = DeploySpot.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                currentPos.x = x;
                currentPos.y = y;

                if (DeploySpot.HasTile(currentPos))
                {
                    tiles.Add(currentPos);
                }
            }
        }

        //foreach (var t in tiles)
        //{
        //    Debug.Log($"{GetType()} - 타일이름 위치 - {t}, {t.x}, {t.y}");
        //}

        return tiles;
    }

}
