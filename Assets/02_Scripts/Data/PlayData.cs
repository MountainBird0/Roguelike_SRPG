using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public bool isSave;
    public int currentStage;
    public int clearCount;

    public int lineCount;
    public Queue<int> iconCounts;
    public Queue<ICON> iconTypes;
    public Queue<Vector2> iconPos;
}

public class IconData
{
    public ICON iconType;
    public Vector2 iconPos;
}