/**********************************************************
* ���� ���� ����(����, ������ ��ų ��)
***********************************************************/
using UnityEngine;

public static class Turn
{
    public static Unit unit;

    public static TileLogic originTile;    // �� ���� �� ������ ��ġ
    public static TileLogic currentTile;   // CAS���� �����̴� ������ ��ġ
    public static TileLogic selectedTile;  // ������ Ÿ��

    public static Vector3Int direction;

    public static SkillData currentSkill;

    public static bool isMoving = false;

    //public static Skill skill;
    //public static Item isItem;
    //public static List<TileLogic> targets;
    //public static bool hasActed; // �ൿ�ߴ���
    //public static bool hasMoved; // �̰� �Ⱦ���
}
