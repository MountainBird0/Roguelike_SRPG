/******************************************************************************
* MainMap 생성
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[DefaultExecutionOrder((int)SEO.MainMapMaker)]
public class MainMapMaker : MonoBehaviour
{
    public Transform map;
    public GameObject line;

    [Header("[ Icon ]")] // 아이콘들
    public GameObject Monster;
    public GameObject Boss;
    public GameObject Shop;
    public GameObject Chest;

    public GameObject Root;

    private MapData mapData;

    private List<GameObject> icons;
    private List<IconNode> nodes;

    private int scaleDuration = 1;
    private Vector2 maxScale = new Vector3(1.3f, 1.3f);

    private void Awake()
    {
        mapData = new MapData();
        nodes = new List<IconNode>();
        icons = new List<GameObject>();
    }

    /**********************************************************
    * 노드 생성
    ***********************************************************/
    public void MakeNode()
    {
        mapData = DataManager.instance.mapData;

        IconNode rootNode = new IconNode(Root);
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
                        // lineRenderer.colorGradient = Color.black;
                        lineRenderer.endColor = Color.black;
                    }
                    else if (nodes[i].connectedNodes[j].iconState == IconState.ATTAINABLE)
                    {
                        lineRenderer.endColor = Color.blue;
                    }
                }
            }
        }
    }



    /**********************************************************
    * 아이콘 생성
    ***********************************************************/
    public void MakeIconTT()
    {
        mapData = DataManager.instance.mapData;

        GameObject icon = null;
        IconType iconType;

        int iconIndex = 0;

        for (int i = 0; i < mapData.lineCount; i++)
        {
            for (int j = 0; j < mapData.iconCounts[i]; j++)
            {
                iconType = mapData.iconInfo[iconIndex].Item1;

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
                    case IconType.CHEST:
                        icon = Instantiate(Chest, map);
                        break;
                }
                icon.transform.position = mapData.iconInfo[iconIndex].Item2;
                icons.Add(icon);



                iconIndex++;
            }
        }
    }


    /**********************************************************
    * 노드 생성 및 연결 후 DataMgr에 저장
    ***********************************************************/
    public void MakeNodeTT()
    {
        IconNode rootNode = new IconNode(Root);
        rootNode.iconState = IconState.VISITED;
        nodes.Add(rootNode);

        for (int i = 0; i < icons.Count; i++)
        {
            IconNode node = new IconNode(icons[i]);
            nodes.Add(node);
            for (int j = 0; j < mapData.nodeDatas[i].Item2; j++)
            {
                nodes[mapData.nodeDatas[i].Item1 + j].AddConnection(node);
            }

            
        }

        DataManager.instance.nodes = nodes;


    }


    /**********************************************************
    * 생성된 노드에 아이콘 넣기
    ***********************************************************/
    public void AddIconToNode()
    {
        nodes = DataManager.instance.nodes;

        for(int i = 1; i < nodes.Count; i++)
        {
            nodes[i].icon = icons[i - 1];
            if (nodes[i].iconState == IconState.ATTAINABLE)
            {
                nodes[i].icon.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }










    











    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ShowMap();
        //}
    }

}
