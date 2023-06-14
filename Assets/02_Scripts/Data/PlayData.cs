/**********************************************************
* �÷��� �߿� ����� �����͵�
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
* ������ �⺻ ���� - �õ�, ���� ��������
***********************************************************/
public class GameInfo
{
    public int seed;
    public int currentStage;
}

/**********************************************************
* ���� �����Ǿ��ִ� ���� ����
***********************************************************/
public class MapData
{
    public int lineCount;    // ���� ��
    public List<int> iconCounts = new List<int>();  // �� ���� �� ������ ��

    public List<(IconType, Vector2)> iconInfo = new List<(IconType, Vector2)>(); // �� �������� ������ ��ġ
    public List<IconState> iconStates = new List<IconState>();

    public List<(int, int)> nodeDatas = new List<(int, int)>();           // ������� ������ġ, ��� ����
}

