using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public int seed;
    public int currentStage;
}

public class MapData
{
    public int lineCount; // 라인 수
    public List<int> iconCounts = new List<int>();  // 한 라인 당 아이콘 수

    public List<(ICON, Vector2)> iconState = new List<(ICON, Vector2)>(); // 각 아이콘의 종류와 위치
    public List<(int, int)> nodeDatas = new List<(int, int)>(); // 이전노드 시작위치, 몇번 들어갈지

    public int[,][] iconGrid; // 아이콘 좌표화와 어느 부모 노드에 들어갈지
}



public class StageDataTempA
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

