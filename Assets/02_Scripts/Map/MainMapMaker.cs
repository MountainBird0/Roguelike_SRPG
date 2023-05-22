/******************************************************************************
* MainMap 积己
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapMaker)]
public class MainMapMaker : MonoBehaviour
{
    public Transform map;

    [Header("[ Icon ]")] // 酒捞能甸
    public GameObject Monster;
    public GameObject Boss;
    public GameObject Shop;
    public GameObject Chest;

    private StageData stageData;

    private void Awake()
    {
        stageData = new StageData()
        {
            iconCounts = new Queue<int>(),
            iconTypes = new Queue<ICON>(),
            iconPos = new Queue<Vector2>()
        };
    }

    public void Start()
    {
        MakeMap();
    }

    /**********************************************************
    * 甘 积己
    ***********************************************************/
    private void MakeMap()
    {
        Debug.Log($"{GetType()} - 甘积己 矫累");

        stageData = DataManager.instance.stageData;

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
            }
        }
    }


}
