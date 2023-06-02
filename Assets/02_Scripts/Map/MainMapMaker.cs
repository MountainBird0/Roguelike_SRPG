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

    private StageDataTempA stageDataTempA;

    private MapData mapData;

    // 노드
    private List<IconNode> nodes;

    private void Awake()
    {
        stageDataTempA = new StageDataTempA();
        mapData = new MapData();
        nodes = new List<IconNode>();
    }

    public void Start()
    {
        MakeMap(); 
    }

    /**********************************************************
    * 맵 생성  
    ***********************************************************/
    private void MakeMap()
    {
        Debug.Log($"{GetType()} - 맵생성 시작");

        mapData = DataManager.instance.mapdata;

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

                MakeNode(i, j, iconIndex, icon);

                iconIndex++;
            }
        }
    }

    private void MakeNode(int lineCount, int iconCount, int iconIndex, GameObject icon)
    {
        // 레벨마다 루트 포지션 변경 추가
        for(int i = 0; i < mapData.nodeDatas[iconIndex].Item2; i++)
        {

        }




    }




    private void MakeMap(int a)
    {
        Debug.Log($"{GetType()} - 맵생성 시작");

        mapData = DataManager.instance.mapdata;

        GameObject icon = null;
        ICON iconType;

        int iconIndex = 0;

        for (int i = 0; i < mapData.lineCount; i++)
        {
            for (int j = 0; j < mapData.iconCounts[i]; j++)
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

                iconIndex++;
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            MakeMap(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            MakeMap(2);
        }
    }



    /**********************************************************
    * 맵 생성
    ***********************************************************/
    private void MakeMapTempA()
    {
        Debug.Log($"{GetType()} - 맵생성 시작");

        stageDataTempA = DataManager.instance.stageDataTempA;

        GameObject icon = null;
        ICON iconType;
        int iconCount;

        for (int i = 0; i < stageDataTempA.lineCount; i++)
        {
            iconCount = stageDataTempA.iconCounts.Dequeue();

            for (int j = 0; j < iconCount; j++)
            {
                iconType = stageDataTempA.iconTypes.Dequeue();

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
                icon.transform.position = stageDataTempA.iconPos.Dequeue();

                // 노드 추가
                // iconNodes[0].children.Add();
                // 노드 추가
            }
        }
    }


}
