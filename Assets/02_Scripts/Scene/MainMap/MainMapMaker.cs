/******************************************************************************
* MainMap 생성
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder((int)SEO.MainMapMaker)]
public class MainMapMaker : MonoBehaviour
{
    public Transform map;
    public GameObject line;

    public GameObject Root;
    
    [Header("[ Icon ]")] // 아이콘들
    public GameObject Monster;
    public GameObject Elite;
    public GameObject Shop;
    public GameObject Chest;
    public GameObject Boss;

    private MapInfo mapData = new();

    private List<IconNode> nodes = new();
    private List<LineRenderer> lines = new();
    
    // 아이콘 움직임
    private int scaleDuration = 1;
    private Vector2 maxScale = new Vector3(1.3f, 1.3f);

    /**********************************************************
    * 노드 생성
    ***********************************************************/
    public void MakeNode()
    {
        mapData = DataManager.instance.mapInfo;

        IconNode rootNode = new IconNode(Root);
        rootNode.iconState = IconState.VISITED;
        nodes.Add(rootNode);

        for (int i = 0; i < mapData.nodeDatas.Count; i++)
        {
            IconNode node = new IconNode(null);
            nodes.Add(node);

            for (int j = 0; j < mapData.nodeDatas[i].Item2; j++)
            {
                nodes[mapData.nodeDatas[i].Item1 + j].AddConnection(node);
            }
            node.iconInfo = mapData.iconInfo[i];

            node.iconState = mapData.iconStates[i];
        }

        DataManager.instance.nodes = nodes;
    }

    /**********************************************************
    * 노드에 맞는 아이콘 생성
    ***********************************************************/
    public void MakeIcon()
    {
        nodes = DataManager.instance.nodes;
     
        GameObject icon = null;
        IconType iconType;

        for (int i = 1; i < nodes.Count; i++)
        {
            iconType = nodes[i].iconInfo.Item1;
            switch (iconType)
            {
                case IconType.MONSTER:
                    icon = Instantiate(Monster, map);
                    break;
                case IconType.SHOP:
                    icon = Instantiate(Shop, map);
                    break;
                case IconType.BOSS:
                    icon = Instantiate(Boss, map);
                    break;
                case IconType.ELITE:
                    icon = Instantiate(Elite, map);
                    break;
                case IconType.CHEST:
                    icon = Instantiate(Chest, map);
                    break;
            }
            icon.transform.position = nodes[i].iconInfo.Item2;
            if(nodes[i].iconState == IconState.ATTAINABLE)
            {
                icon.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            }
            nodes[i].icon = icon;
        }
    }

    /**********************************************************
    * 노드기반으로 라인 그리기
    ***********************************************************/
    public void DrawLine()
    {
        for (int i = 1; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes[i].connectedNodes.Count; j++)
            {
                var lineObject = Instantiate(line);
                var lineRenderer = lineObject.GetComponent<LineRenderer>();

                var fromPoint = nodes[i].icon.transform.position;
                var toPoint = nodes[i].connectedNodes[j].icon.transform.position;
                lineRenderer.SetPosition(0, fromPoint);
                lineRenderer.SetPosition(1, toPoint);

                if (nodes[i].iconState == IconState.VISITED)
                {
                    if (nodes[i].connectedNodes[j].iconState == IconState.VISITED)
                    {
                        lineRenderer.endColor = Color.black;
                    }
                    else if (nodes[i].connectedNodes[j].iconState == IconState.ATTAINABLE)
                    {
                        lineRenderer.endColor = Color.blue;
                    }
                }
                lines.Add(lineRenderer);

            }
        }
    }

    public void ChangeLineColor()
    {
        int count = 0;

        for (int i = 1; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes[i].connectedNodes.Count; j++)
            {
                if (nodes[i].iconState == IconState.VISITED)
                {  
                    if (nodes[i].connectedNodes[j].iconState == IconState.VISITED)
                    {
                        lines[count].endColor = Color.black;
                    }
                    else if (nodes[i].connectedNodes[j].iconState == IconState.ATTAINABLE)
                    {
                        lines[count].endColor = Color.blue;
                    }
                    else
                    {
                        lines[count].endColor = Color.white;
                    }
                }
                count++;
            }
        }
    }
}
