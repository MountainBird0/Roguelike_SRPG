/**********************************************************
* ���� �����Ǿ��ִ� ���� ���� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public List<(IconType, Vector2)> iconInfoList = new(); // �� �������� ������ ��ġ
    public List<IconState> iconStates = new();             

    public List<(int, int)> nodeData = new();         // ���� ��� �ε���, ���� � �׸���
}

