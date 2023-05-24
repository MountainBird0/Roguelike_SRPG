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

    private StageDataTempA stageDataTempA;

    private MapData mapData;

    // ��� �߰��غ���
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
    * �� ����  
    ***********************************************************/
    private void MakeMap()
    {
        Debug.Log($"{GetType()} - �ʻ��� ����");

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
    * �� ����
    ***********************************************************/
    private void MakeMapTempA()
    {
        Debug.Log($"{GetType()} - �ʻ��� ����");

        stageDataTempA = DataManager.instance.stageDataTempA;

        // �ʿ� �Ⱥ��̴� root�ϳ� �߰��ʿ�
        // ��� �߰�
        iconNodes = new List<IconNode>();
        // ��� �߰�

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

                // ��� �߰�
                // iconNodes[0].children.Add();
                // ��� �߰�
            }
        }
    }


}
