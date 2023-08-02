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

    public Dictionary<Vector3Int, TileLogic> mainTiles = new();
    public Dictionary<Vector3Int, TileLogic> highlightTiles = new();

    [Header("Maker")]
    public MonsterMaker monsterMaker;

    private Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    private void Awake()
    {
        SetTiles();
    }

    private void Start()
    {
        monsterMaker.CreateMonters(mainTiles);
    }

    /**********************************************************
    * Ÿ�� Dictionary ����
    ***********************************************************/
    private void SetTiles()
    {
        SetTile(mainMap, mainTiles);
        
    }
    public void SetTile(Tilemap map, Dictionary<Vector3Int, TileLogic> tiles)
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
                    TileLogic tileLogic = new TileLogic(currentPos);
                    tiles.Add(currentPos, tileLogic);
                }
            }
        }
    }


    /**********************************************************
    * ���̶���Ʈ Ÿ�� ���� / �����
    ***********************************************************/
    public void ShowHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlightMap.SetTile(tiles[i].pos, blueHighlightTile);
        }
        SetTile(highlightMap, highlightTiles);

    }
    public void ClearHighTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlightMap.SetTile(tiles[i].pos, null);
        }
        highlightTiles.Clear();
    }


    /**********************************************************
    * Ÿ�� ������ �´� Ÿ�� ����Ʈ�� ��ȯ
    ***********************************************************/
    public List<TileLogic> Search(TileLogic start, Func<TileLogic, TileLogic, bool> searchType)
    {
        List<TileLogic> tilesResult = new List<TileLogic>(); // ��� ��ȯ�� Ÿ��

        tilesResult.Add(start);
        ClearSearch(); // logic.prev �ʱ�ȭ, distance �� ũ��

        Queue<TileLogic> checkNext = new Queue<TileLogic>();
        Queue<TileLogic> checkNow = new Queue<TileLogic>();

        start.distance = 0;
        checkNow.Enqueue(start); // ���� ��ġ�� ����

        while (checkNow.Count > 0)
        {
            TileLogic t = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                // maintiles���� ���� Ÿ���� �����¿� Ÿ���� ������ ��
                TileLogic next = GetTile(t.pos + dirs[i]); 

                if (next == null || next.distance <= t.distance + 1)
                {
                    // ����Ÿ���� ���ų� ���� Ÿ�ϰ��� �Ÿ��� 1 �̻� ���̳��ٸ� 
                    continue;
                }
                if (searchType(t, next))
                {
                    next.prev = t; // �̰� ����? ��ã������?
                    checkNext.Enqueue(next);
                    tilesResult.Add(next);
                }
            }

            if (checkNow.Count == 0)
            {
                SwapReference(ref checkNow, ref checkNext);
            }
        }
        return tilesResult;
    }
    private void SwapReference(ref Queue<TileLogic> now, ref Queue<TileLogic> next)
    {
        // �׳� next clear�ϸ�?
        Queue<TileLogic> temp = now;
        now = next;
        next = temp;
    }
    private void ClearSearch()
    {
        foreach (TileLogic t in mainTiles.Values)
        {
            t.prev = null;
            t.distance = int.MaxValue;
        }
    }
    public TileLogic GetTile(Vector3Int pos)
    {
        TileLogic tile = null;
        mainTiles.TryGetValue(pos, out tile);

        return tile;
    }
}
