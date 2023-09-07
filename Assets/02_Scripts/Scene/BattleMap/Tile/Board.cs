/**********************************************************
* �� Ÿ���� ���� �� 
***********************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Grid grid;

    [Header("Tile")]
    public Tile blueHighlightTile;
    public Tile redHighlightTile;
    public Tile redAimingTile;
    public Tile greenAimingTile;
    public List<Tile> arrowTiles;

    [Header("TileMap")]
    public Tilemap mainMap;
    public Tilemap highlightMap;
    public Tilemap deployMap;
    public Tilemap aimingMap;

    public Dictionary<Vector3Int, TileLogic> mainTiles = new();
    public Dictionary<Vector3Int, TileLogic> highlightTiles = new();
    public Dictionary<Vector3Int, TileLogic> aimingTiles = new();

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
        SetTile(mainMap, mainTiles);
    }

    private void Start()
    {
        monsterMaker.CreateMonters(mainTiles);
    }

    /**********************************************************
    * Ÿ�� Dictionary ����
    ***********************************************************/
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
    * �̵� Ÿ�� ���̱� / �����
    ***********************************************************/
    public void ShowMovableTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            highlightMap.SetTile(tiles[i].pos, blueHighlightTile);
        }
        SetTile(highlightMap, highlightTiles);
    }
    /**********************************************************
    * ��ų ���� Ÿ�� ���̱� / �����
    ***********************************************************/
    public void ShowSkillRangeTile(List<TileLogic> tiles)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if(mainTiles.ContainsKey(tiles[i].pos))
            {
                highlightMap.SetTile(tiles[i].pos, redHighlightTile);

                if(mainTiles[tiles[i].pos].content != null)
                {
                    if (mainTiles[Turn.originTile.pos].content.GetComponent<Unit>().faction != 
                        mainTiles[tiles[i].pos].content.GetComponent<Unit>().faction)
                    {
                        aimingMap.SetTile(tiles[i].pos, redAimingTile);
                    }
                }
            }
        }
        SetTile(highlightMap, highlightTiles);
        
    }
    public void ClearTile()
    {
        highlightMap.ClearAllTiles();
        highlightTiles.Clear();
        
        aimingMap.ClearAllTiles();
        aimingTiles.Clear();
    }

    /**********************************************************
    * Ÿ�� Ÿ�� ���̱� / �����
    ***********************************************************/
    public void ShowAimingTile(List<TileLogic> tiles)
    {
        AffectType affectType = (AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true);

        if (Turn.targets != null)
        {
            Turn.targets.Clear();
        }

        for (int i = 0; i < tiles.Count; i++)
        {

            if (mainTiles[tiles[i].pos].content != null)
            {
                var originUnit = mainTiles[Turn.originTile.pos].content.GetComponent<Unit>();
                var targetUnit = mainTiles[tiles[i].pos].content.GetComponent<Unit>();

                switch (affectType)
                {
                    case AffectType.ALL:
                        break;

                    case AffectType.ALLY:
                        if (originUnit.faction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, redAimingTile);
                            Turn.targets.Add(targetUnit);
                        }
                        break;

                    case AffectType.ENEMY:
                        if (!originUnit.faction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, redAimingTile);
                            Turn.targets.Add(targetUnit);
                        }
                        break;

                    case AffectType.SELF:
                        break;
                }
            }
        }
        SetTile(aimingMap, aimingTiles);
    }


    /**********************************************************
    * Ÿ�� ������ �´� Ÿ�� ����Ʈ�� ��ȯ
    ***********************************************************/
    public List<TileLogic> Search(TileLogic start, int range, Func<TileLogic, TileLogic, int, bool> searchType)
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
                if (searchType(t, next, range))
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

    /**********************************************************
    * pos�� ��ġ�� TileLogic ��ȯ
    ***********************************************************/
    public TileLogic GetTile(Vector3Int pos)
    {
        TileLogic tile = null;
        mainTiles.TryGetValue(pos, out tile);

        return tile;
    }

    /**********************************************************
    * ȭ��ǥ Ÿ�� ���� / ����
    ***********************************************************/
    public void ShowArrowTile(Vector3Int pos)
    {
        Vector3Int currentPos;

        for (int i = 0; i < 4; i++)
        {
            currentPos = pos + dirs[i];
            aimingMap.SetTile(currentPos, arrowTiles[i]);

            TileLogic tileLogic = new TileLogic(currentPos); // ����?
            aimingTiles.Add(currentPos, tileLogic);
        }
    }
    public void ClearArrowTile()
    {
        aimingMap.ClearAllTiles();
        aimingTiles.Clear();
    }





}
