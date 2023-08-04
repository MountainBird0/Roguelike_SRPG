using System.Collections.Generic;
using UnityEngine;

/**********************************************************
* ���� �����Ǿ��ִ� ���� ����
***********************************************************/
public class MapInfo
{
    public List<(IconType, Vector2)> iconInfo = new(); // �� �������� ������ ��ġ
    public List<IconState> iconStates = new();

    public List<(int, int)> nodeDatas = new();           // ������� ������ġ, ��� ����
}
