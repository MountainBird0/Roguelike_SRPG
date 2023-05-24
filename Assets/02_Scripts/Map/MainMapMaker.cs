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

    private StageDataTempA stageDataTempA;

    private MapData mapData;

    // 노드 추가해보기
    private List<IconNode> iconNodes;

    private void Awake()
    {
        stageDataTempA = new StageDataTempA();
        mapData = new MapData();
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

        mapData = DataManager.instance.mapdatas[GameManager.instance.currentStage - 1];

        GameObject icon = null;
        ICON iconType;

        int iconIndex = 0;

        for(int i = 0; i < mapData.lineCount; i++)
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

                iconIndex++;
            }
        }
    }


    /**********************************************************
    * 맵 생성
    ***********************************************************/
    private void MakeMapTempA()
    {
        Debug.Log($"{GetType()} - 맵생성 시작");

        stageDataTempA = DataManager.instance.stageDataTempA;

        // 맵에 안보이는 root하나 추가필요
        // 노드 추가
        iconNodes = new List<IconNode>();
        // 노드 추가

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
