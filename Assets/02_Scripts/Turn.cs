/**********************************************************
* ���� ���� ����(����, ������ ��ų ��)
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    // ��ü
    public static int turnCount = 1;
    public static bool isHumanTurn = true;
    
    
    // ���� �����̴� ���ֿ� ����
    public static Unit unit;

    public static Vector3Int originPos;    // �� ���� �� ������ ��ġ
    public static Vector3Int currentPos;   // CAS���� ������ ������ ��ġ, ���� �������� �� ��
    public static Vector3Int selectedPos;  // ������ Ÿ��

    public static Vector3Int direction;

    public static Skill skill;

    public static bool isMoving = false;

    // �� �� �ϳ� �� ��
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
    //public static bool hasActed; // �ൿ�ߴ���

    //public static bool hasMoved; // �̰� �Ⱦ���
}
