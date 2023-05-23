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

    private StageData stageData;

    // 노드 추가해보기
    private List<IconNode> iconNodes;

    private void Awake()
    {
        stageData = new StageData();
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

        stageData = DataManager.instance.stageData;

        // 맵에 안보이는 root하나 추가필요
        // 노드 추가
        iconNodes = new List<IconNode>();
        // 노드 추가

        GameObject icon = null;
        ICON iconType;
        int iconCount;

        for (int i = 0; i < stageData.lineCount; i++)
        {
            iconCount = stageData.iconCounts.Dequeue();

            for (int j = 0; j < iconCount; j++)
            {
                iconType = stageData.iconTypes.Dequeue();

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
                icon.transform.position = stageData.iconPos.Dequeue();

                // 노드 추가
                // iconNodes[0].children.Add();
                // 노드 추가
            }
        }
    }


}
