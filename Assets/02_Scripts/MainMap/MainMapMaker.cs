/******************************************************************************
* MainMap 생성
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        mapData = new MapData();
        nodes = new List<IconNode>();
        icons = new List<GameObject>();
    }

    /**********************************************************
    * 아이콘 생성
    ***********************************************************/
    public void MakeIcon()
    {
        mapData = DataManager.instance.mapData;

        GameObject icon = null;
        Icon iconType;

        int iconIndex = 0;

        for (int i = 0; i < mapData.lineCount; i++)
        {
            for (int j = 0; j < mapData.iconCounts[i]; j++)
            {
                iconType = mapData.iconState[iconIndex].Item1;

                switch (iconType)
                {
                    case Icon.MONSTER:
                        icon = Instantiate(Monster, map);
                        break;
                    case Icon.SHOP:
                        icon = Instantiate(Shop, map);
                        break;
                    case Icon.BOSS:
                        icon = Instantiate(Boss, map);
                        break;
                    case Icon.CHEST:
                        icon = Instantiate(Chest, map);
                        break;
                }
                icon.transform.position = mapData.iconState[iconIndex].Item2;
                icons.Add(icon);

                iconIndex++;
            }
        }
    }


    /**********************************************************
    * 노드 생성 및 연결 후 DataMgr에 저장
    ***********************************************************/
    public void MakeNode()
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
                        Debug.Log($"{GetType()} - 그림?");
                        // lineRenderer.colorGradient = Color.black;
                        lineRenderer.endColor = Color.black;
                    }
                    else if (nodes[i].connectedNodes[j].iconState == IconState.ATTAINABLE)
                    {
                        Debug.Log($"{GetType()} - 그림?");
                        lineRenderer.endColor = Color.blue;
                    }
                }
            }
        }
    }







    /**********************************************************
    * 맵 생성 -- 예전
    ***********************************************************/
    public void MakeMap()
    {
        Debug.Log($"{GetType()} - 맵생성 시작");

        mapData = DataManager.instance.mapData;

        GameObject icon = null;
        Icon iconType;

        int iconIndex = 0;

        IconNode rootNode = new IconNode(Root);
        nodes.Add(rootNode);

        for (int i = 0; i < mapData.lineCount; i++)
        {
            for(int j = 0; j < mapData.iconCounts[i]; j++)
            {
                iconType = mapData.iconState[iconIndex].Item1;
                switch (iconType)
                {
                    case Icon.MONSTER:
                        icon = Instantiate(Monster, map);
                        break;
                    case Icon.SHOP:
                        icon = Instantiate(Shop, map);
                        break;
                    case Icon.BOSS:
                        icon = Instantiate(Boss, map);
                        break;
                    case Icon.CHEST:
                        icon = Instantiate(Chest, map);
                        break;
                }
                icon.transform.position = mapData.iconState[iconIndex].Item2;
                

                IconNode node = new IconNode(icon);
                nodes.Add(node);
                MakeNodea(iconIndex, node);

                iconIndex++;
            }
        }

        foreach(var a in mapData.nodeDatas)
        {
            Debug.Log($"{GetType()} - 노드 생성 보기 : {a.Item1}, {a.Item2} ");
        }

        DataManager.instance.nodes = nodes;
    }


    /**********************************************************
    * 노드 연결
    ***********************************************************/
    private void MakeNodea(int iconIndex, IconNode node)
    {
        for (int i = 0; i < mapData.nodeDatas[iconIndex].Item2; i++)
        {
            nodes[mapData.nodeDatas[iconIndex].Item1 + i].AddConnection(node);
        }
    }


    /**********************************************************
    * 마지막 VISITED 노드 찾아서 자식 ATTAINABLE로
    ***********************************************************/






    /**********************************************************
    * 저장된거 기반으로 불러오기 - 노드 기반
    ***********************************************************/
    private void ShowMap()
    {
        nodes = DataManager.instance.nodes;

        GameObject icon;

        for (int i = 0; i < nodes.Count; i++)
        {
            icon = Instantiate(nodes[i].icon, map);
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
