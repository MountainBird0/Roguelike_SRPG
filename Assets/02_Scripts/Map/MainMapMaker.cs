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

    private List<IconNode> nodes;   

    private void Awake()
    {
        mapData = new MapData();
        nodes = new List<IconNode>();
    }


    /**********************************************************
    * �� ����  
    ***********************************************************/
    public void MakeMap()
    {
        Debug.Log($"{GetType()} - �ʻ��� ����");

        mapData = DataManager.instance.mapData;

        GameObject icon = null;
        ICON iconType;

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
                    case ICON.MONSTER:
                        icon = Instantiate(Monster, map);
                        break;
                    case ICON.SHOP:
                        icon = Instantiate(Shop, map);
                        break;
                    case ICON.BOSS:
                        icon = Instantiate(Boss, map);
                        break;
                    case ICON.CHEST:
                        icon = Instantiate(Chest, map);
                        break;
                }
                icon.transform.position = mapData.iconState[iconIndex].Item2;

                IconNode node = new IconNode(icon);
                nodes.Add(node);
                MakeNode(iconIndex, icon, node);

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
    private void MakeNode(int iconIndex, GameObject icon, IconNode node)
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
