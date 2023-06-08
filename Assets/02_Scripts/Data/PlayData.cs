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

    public List<(Icon, Vector2)> iconState = new List<(Icon, Vector2)>(); // �� �������� ������ ��ġ
    public List<(int, int)> nodeDatas = new List<(int, int)>();           // ������� ������ġ, ��� ����
}

