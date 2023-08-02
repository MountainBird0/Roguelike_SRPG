using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Turn
{
    public static Unit unit;
    public static TileLogic prevTile;
    public static TileLogic selectedTile;

    public static bool hasMoved = false;

    //public static Skill skill;
    //public static Item isItem;
    //public static List<TileLogic> targets;
    //public static bool hasActed; // 행동했는지
    //public static bool hasMoved; // 이건 안쓸듯
}
