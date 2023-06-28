/**********************************************************
* ���θʿ� �����Ǵ� �������� Ʋ
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class IconNode
{
    public GameObject icon;
    public (IconType, Vector2) iconInfo; // �� �������� ������ ��ġ
    public IconState iconState = IconState.LOCKED;

    public List<IconNode> connectedNodes;

    public IconNode(GameObject icon)
    {
        this.icon = icon;
        connectedNodes = new List<IconNode>();
    }

    public void AddConnection(IconNode node)
    {
        connectedNodes.Add(node);
    }
}