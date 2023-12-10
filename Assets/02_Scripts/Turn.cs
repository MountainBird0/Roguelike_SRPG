/**********************************************************
* 현재 턴의 정보(유닛, 선택한 스킬 등)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    // 전체
    public static int turnCount = 1;
    public static bool isHumanTurn = true;
    
    
    // 지금 움직이는 유닛에 대해
    public static Unit unit;

    public static Vector3Int originPos;    // 턴 시작 시 유닛의 위치
    public static Vector3Int currentPos;   // CAS에서 움직인 유닛의 위치, 방향 선택했을 때 등
    public static Vector3Int selectedPos;  // 선택한 타일

    public static Vector3Int direction;

    public static Skill skill;

    public static bool isMoving = false;

    // 둘 중 하나 쓸 듯
    //public static List<TileLogic> targetTiles = new();
    public static List<Unit> targets = new();

    public static void Clear()
    {
        unit = null;
        originPos = Vector3Int.zero;
        currentPos = Vector3Int.zero;
        selectedPos = Vector3Int.zero;

        direction = Vector3Int.zero;

        isMoving = false;

        targets.Clear();
    }



    //public static Skill skill;
    //public static Item isItem;
    //public static bool hasActed; // 행동했는지

    //public static bool hasMoved; // 이건 안쓸듯
}
