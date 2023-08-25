/**********************************************************
* 현재 턴의 정보(유닛, 선택한 스킬 등)
***********************************************************/
public static class Turn
{
    public static Unit unit;
    public static TileLogic prevTile;
    public static TileLogic selectedTile;

    public static SkillData currentSkill;

    public static bool isMoving = false;

    //public static Skill skill;
    //public static Item isItem;
    //public static List<TileLogic> targets;
    //public static bool hasActed; // 행동했는지
    //public static bool hasMoved; // 이건 안쓸듯
}
