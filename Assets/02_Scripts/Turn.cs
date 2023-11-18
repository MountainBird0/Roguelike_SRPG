/**********************************************************
* ���� ���� ����(����, ������ ��ų ��)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    // ��ü
    public static int turnCount = 0;
    public static bool isHumanTurn = true;
    
    
    // ���� �����̴� ���ֿ� ����
    public static Unit unit;

    public static TileLogic originTile;    // �� ���� �� ������ ��ġ
    public static TileLogic currentTile;   // CAS���� �����̴� ������ ��ġ
    public static TileLogic selectedTile;  // ������ Ÿ��

    public static Vector3Int direction;

    public static int skillSlotNum;
    public static SkillData currentSkill;

    public static bool hasMoved = false;

    // �� �� �ϳ� �� ��
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
    //public static bool hasActed; // �ൿ�ߴ���

    //public static bool hasMoved; // �̰� �Ⱦ���
}
