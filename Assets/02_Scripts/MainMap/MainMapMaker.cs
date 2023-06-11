/******************************************************************************
* MainMap ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapMaker)]
public class MainMapMaker : MonoBehaviour
{
    public Transform map;

    [Header("[ Icon ]")] // �����ܵ�
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
    * ������ ����
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
    * ��� ���� �� ����
    ***********************************************************/
    public void MakeNode()
    {
        IconNode rootNode = new IconNode(Root);
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
    * ������ ��忡 ������ �ֱ�
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
    * �� ���� -- ����
    ***********************************************************/
    public void MakeMap()
    {
        Debug.Log($"{GetType()} - �ʻ��� ����");

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
            Debug.Log($"{GetType()} - ��� ���� ���� : {a.Item1}, {a.Item2} ");
        }

        DataManager.instance.nodes = nodes;
    }


    /**********************************************************
    * ��� ����
    ***********************************************************/
    private void MakeNodea(int iconIndex, IconNode node)
    {
        for (int i = 0; i < mapData.nodeDatas[iconIndex].Item2; i++)
        {
            nodes[mapData.nodeDatas[iconIndex].Item1 + i].AddConnection(node);
        }
    }









    /**********************************************************
    * ����Ȱ� ������� �ҷ����� - ��� ���
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