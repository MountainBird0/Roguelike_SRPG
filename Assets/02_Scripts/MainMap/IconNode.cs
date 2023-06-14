/**********************************************************
* 메인맵에 생성되는 아이콘의 틀
***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconNode
{
    public GameObject icon;
    public (IconType, Vector2) iconInfo; // 각 아이콘의 종류와 위치
    public IconState iconState;

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
