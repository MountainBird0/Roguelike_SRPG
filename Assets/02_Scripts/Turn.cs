/**********************************************************
* 현재 턴의 정보(유닛, 선택한 스킬 등)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    // 전체
    public static int turnCount = 0;
    public static bool isHumanTurn = true;
    
    
    // 지금 움직이는 유닛에 대해
    public static Unit unit;

    public static TileLogic originTile;    // 턴 시작 시 유닛의 위치
    public static TileLogic currentTile;   // CAS에서 움직이는 유닛의 위치
    public static TileLogic selectedTile;  // 선택한 타일

    public static Vector3Int direction;

    public static int skillSlotNum;
    public static SkillData currentSkill;

    public static bool hasMoved = false;

    // 둘 중 하나 쓸 듯
    //public static List<TileLogic> targetTiles = new();
    public static List<Unit> targets = new();

    public static void Clear()
    {
        unit = null;
        originTile = null;
        currentTile = null;
        selectedTile = null;

        direction = Vector3Int.zero;

        skillSlotNum = -1;

        currentSkill = null;

        hasMoved = false;

        targets = null;
    }



    //public static Skill skill;
    //public static Item isItem;
    //public static bool hasActed; // 행동했는지

    //public static bool hasMoved; // 이건 안쓸듯
}
