/**********************************************************
* ���� ���� ����(����, ������ ��ų ��)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    public static Unit unit;

    public static TileLogic originTile;    // �� ���� �� ������ ��ġ
    public static TileLogic currentTile;   // CAS���� �����̴� ������ ��ġ
    public static TileLogic selectedTile;  // ������ Ÿ��

    public static Vector3Int direction;

    public static int slotNum;
    public static SkillData currentSkill;

    public static bool isMoving = false;

    // �� �� �ϳ� �� ��
    public static List<TileLogic> targetTiles = new();
    public static List<Unit> targets = new();



    //public static Skill skill;
    //public static Item isItem;
    //public static bool hasActed; // �ൿ�ߴ���

    //public static bool hasMoved; // �̰� �Ⱦ���
}
