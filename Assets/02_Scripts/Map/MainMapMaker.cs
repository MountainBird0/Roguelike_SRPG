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

    private StageData stageData;

    // ��� �߰��غ���
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
    * �� ����
    ***********************************************************/
    private void MakeMap()
    {
        Debug.Log($"{GetType()} - �ʻ��� ����");

        stageData = DataManager.instance.stageData;

        // �ʿ� �Ⱥ��̴� root�ϳ� �߰��ʿ�
        // ��� �߰�
        iconNodes = new List<IconNode>();
        // ��� �߰�

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

                // ��� �߰�
                // iconNodes[0].children.Add();
                // ��� �߰�
            }
        }
    }


}
