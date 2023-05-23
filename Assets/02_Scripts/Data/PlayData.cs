using System.Collections.Generic;
using UnityEngine;

public class StageData
{
    public bool isSave;
    public int currentStage;
    public int clearCount;

    public int lineCount;
    public Queue<int> iconCounts = new Queue<int>();

    public Queue<ICON> iconTypes = new Queue<ICON>();

    public Queue<Vector2> iconPos = new Queue<Vector2>();
    public int[,][] iconGrid;
}

public class IconData
{
    public ICON iconType;
    public Vector2 iconPos;
    public bool isTouchable; // true면 클릭 가능하도록 활성화
}

