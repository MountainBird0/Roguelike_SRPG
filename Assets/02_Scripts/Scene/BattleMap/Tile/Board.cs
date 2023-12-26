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
    public List<Tile> highTiles;
    public List<Tile> aimTiles;
    public List<Tile> arrowTiles;

    public Tile blueHighlightTile;
    public Tile redHighlightTile;
    public Tile yellowHighlightTile;
    public Tile yellowSelectableTile;
    public Tile redAimingTile;
    public Tile greenAimingTile;

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
    * ���̶���Ʈ Ÿ�� ���̱� / �����
    ***********************************************************/
    public void ShowHighlightTile(List<TileLogic> tiles, int num)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if(mainTiles.ContainsKey(tiles[i].pos))
            {
                highlightMap.SetTile(tiles[i].pos, highTiles[num]);
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
    public void ShowAimingTile(List<TileLogic> tiles, int num)
    {
        AffectType affectType = Turn.skill.data.affectType;

        int originFaction = Turn.unit.faction;

        for (int i = 0; i < tiles.Count; i++)
        {

            if (mainTiles[tiles[i].pos].content != null)
            {
                
                var targetUnit = mainTiles[tiles[i].pos].content.GetComponent<Unit>();

                switch (affectType)
                {
                    case AffectType.ALL:
                        break;

                    case AffectType.ALLY:
                        if (originFaction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, aimTiles[num]);
                        }
                        break;

                    case AffectType.HEAL:
                        if (originFaction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, aimTiles[num]);
                        }
                        break;

                    case AffectType.BUFF:
                        if (originFaction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, aimTiles[num]);
                        }
                        break;

                    case AffectType.ENEMY:
                        if (!originFaction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, aimTiles[num]);
                        }
                        break;

                    case AffectType.ATTACK:
                        if (!originFaction.Equals(targetUnit.faction))
                        {
                            aimingMap.SetTile(tiles[i].pos, aimTiles[num]);
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
            TileLogic now = checkNow.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                // maintiles���� ���� Ÿ���� �����¿� Ÿ���� ������ ��
                TileLogic next = GetTile(now.pos + dirs[i]); 

                if (next == null || next.distance <= now.distance + 1)
                {                              
                    continue; // ����Ÿ���� ���ų� ���� Ÿ�ϰ��� �Ÿ��� 1 �̻� ���̳��ٸ� 
                }
                if (searchType(now, next, range))
                {
                    next.prev = now; // �̰� ����? ��ã������?
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
    public bool ISMovable(TileLogic from, TileLogic to, int range)
    {
        to.distance = from.distance + 1;

        return (to.content == null && to.distance <= range);
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


    /**********************************************************
    * �ִ� �ּҹ��� ��ȯ
    ***********************************************************/




    /**********************************************************
    * �ִܰ�� �����
    ***********************************************************/
    public List<TileLogic> checkss(Vector3Int currentPos, Vector3Int selectedPos)
    {
        List<TileLogic> tilesResult = new List<TileLogic>(); // ��� ��ȯ�� Ÿ��



        return tilesResult;
    }


}
