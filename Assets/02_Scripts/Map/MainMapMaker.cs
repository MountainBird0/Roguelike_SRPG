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

    [Header("[ Icon ]")] // 아이콘들
    public GameObject Monster;
    public GameObject Boss;
    public GameObject Shop;
    public GameObject Chest;

    public GameObject Root;

    private MapData mapData;

    private List<IconNode> nodes;   

    private void Awake()
    {
        mapData = new MapData();
        nodes = new List<IconNode>();
    }


    /**********************************************************
    * 맵 생성  
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
                MakeNode(iconIndex, node);

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
    private void MakeNode(int iconIndex, IconNode node)
    {
        for (int i = 0; i < mapData.nodeDatas[iconIndex].Item2; i++)
        {
            nodes[mapData.nodeDatas[iconIndex].Item1 + i].AddConnection(node);
        }
    }

    // 노드 새로 만들기, 만들어진 노드에 icon만 넣기







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
