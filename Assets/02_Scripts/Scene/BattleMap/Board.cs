using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public TilemapRenderer tilemapRenderer;
    public Tilemap tilemap;

    public Vector2Int minXY;
    public Vector2Int maxXY;

    private void Awake()
    {
        tilemapRenderer = this.transform.GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        // LoadTiles();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);

            if (tile != null)
            {
                // Ÿ�� ��ġ�� ������ ����ϴ� ���� �߰�
                Debug.Log("Ÿ�� ��ġ: " + position);
                Debug.Log("Ÿ�� ����: " + tile.name);
            }
        }
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
            Debug.Log($"{GetType()} - Ÿ���̸� ��ġ - {t}, {t.x}, {t.y}");
        }

        return tiles;
    }

}
