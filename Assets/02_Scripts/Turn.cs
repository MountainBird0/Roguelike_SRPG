/**********************************************************
* 현재 턴의 정보(유닛, 선택한 스킬 등)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    public static Unit unit;

    public static TileLogic originTile;    // 턴 시작 시 유닛의 위치
    public static TileLogic currentTile;   // CAS에서 움직이는 유닛의 위치
    public static TileLogic selectedTile;  // 선택한 타일

    public static Vector3Int direction;

    public static int slotNum;
    public static SkillData currentSkill;

    public static bool isMoving = false;

    // 둘 중 하나 쓸 듯
    public static List<TileLogic> targetTiles = new();
    public static List<Unit> targets = new();



    //public static Skill skill;
    //public static Item isItem;
    //public static bool hasActed; // 행동했는지

    //public static bool hasMoved; // 이건 안쓸듯
}
