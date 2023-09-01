/**********************************************************
* 현재 턴의 정보(유닛, 선택한 스킬 등)
***********************************************************/
using UnityEngine;

public static class Turn
{
    public static Unit unit;

    public static TileLogic originTile;    // 턴 시작 시 유닛의 위치
    public static TileLogic currentTile;   // CAS에서 움직이는 유닛의 위치
    public static TileLogic selectedTile;  // 선택한 타일

    public static Vector3Int direction;

    public static SkillData currentSkill;

    public static bool isMoving = false;

    //public static Skill skill;
    //public static Item isItem;
    //public static List<TileLogic> targets;
    //public static bool hasActed; // 행동했는지
    //public static bool hasMoved; // 이건 안쓸듯
}
