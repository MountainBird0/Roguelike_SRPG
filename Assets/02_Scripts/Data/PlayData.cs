using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public int seed;
    public int currentStage;
}

public class MapData
{
    public int lineCount; // ���� ��
    public List<int> iconCounts = new List<int>();  // �� ���� �� ������ ��

    public List<(ICON, Vector2)> iconState = new List<(ICON, Vector2)>(); // �� �������� ������ ��ġ
    public List<(int, int)> nodeDatas = new List<(int, int)>(); // ������� ������ġ, ��� ����

    public int[,][] iconGrid; // ������ ��ǥȭ�� ��� �θ� ��忡 ����
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
    public bool isTouchable; // true�� Ŭ�� �����ϵ��� Ȱ��ȭ
}

