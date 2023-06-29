using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap hightlight;

    public Vector2Int minXY;
    public Vector2Int maxXY;

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
        for (int i = minXY.x; i <= maxXY.x; i++)
        {
            for (int j = minXY.y; j <= maxXY.y; j++)
            {
                Vector3Int currentPos = new Vector3Int(i, j, 0);
                if (tilemap.HasTile(currentPos))
                {
                    tiles.Add(currentPos);
                }
            }
        }


        foreach(var t in tiles)
        {
            Debug.Log($"{GetType()} - 타일이름 위치 - {t}, {t.x}, {t.y}");
        }

        return tiles;
    }

}
