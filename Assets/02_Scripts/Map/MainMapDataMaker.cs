/**********************************************************
* ���� �� ���� �����͸� ������
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapDataMaker)]
public class MainMapDataMaker : MonoBehaviour
{
    private StageLevelData stageLevel;
    private IconProbabilityData iconProbability;
    private StageData stageData;
    private Dictionary<ICON, int> probabilityMap;

    private List<IconTree> iconTrees;

    private int currentStage;

    private float mapWidth;
    private float mapHeight;

    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        stageData = new StageData()
        {
            iconCounts = new Queue<int>(),
            iconTypes = new Queue<ICON>(),
            iconPos = new Queue<Vector2>()
        };
        probabilityMap = new Dictionary<ICON, int>();
        iconTrees = new List<IconTree>();
    }

    private void Start()
    {
        // �����ϱ� �ǹ�
        if (!stageData.isSave)
        {
            Debug.Log($"{GetType()} - ���ο� �� ������ ����");
            MakeMapData();
        }
    }


    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    * - ������ ó�� ������ ��, �� stage�� Ŭ���� ���� �� ���
    * - ���� ����Ȯ�� �߰� �ʿ�
    ***********************************************************/
    public void MakeMapData()
    {
        SetData();

        int iconCount;

        int currentShopCount = 0;

        Vector2 iconPos;
        iconPos.x = -(mapWidth / 2);
        float widthGap = mapWidth / (stageLevel.lineCount - 1);
        float heightGap;

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if (i == 0) 
            {
                iconTrees = new List<IconTree>(stageLevel.firstLine);

                stageData.iconCounts.Enqueue(stageLevel.firstLine);

                heightGap = mapHeight / (stageLevel.firstLine + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;

                for (int j = 0; j < stageLevel.firstLine; j++)
                {
                    //��� ����?
                    if( j == 0)
                    {

                        // ���� j = 0�� ����
                    }
                    else if(j == stageLevel.firstLine - 1)
                    {
                        // ���� j = stageLevel.firstLine - 1�� ����
                    }
                    else
                    {

                    }



                    stageData.iconPos.Enqueue(iconPos);
                    iconPos.y += heightGap;
                    stageData.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // ������ ����
            else if (i == (stageLevel.lineCount - 1))
            {
                stageData.iconCounts.Enqueue(stageLevel.lastLine);

                heightGap = mapHeight / (stageLevel.lastLine + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;

                for (int j = 0; j < stageLevel.lastLine; j++)
                {
                    stageData.iconPos.Enqueue(iconPos);
                    iconPos.y += heightGap;
                    stageData.iconTypes.Enqueue(ICON.BOSS);
                }
            }
            else
            {
                iconCount = Random.Range(2, 5);
                stageData.iconCounts.Enqueue(iconCount);

                heightGap = mapHeight / (iconCount + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageData.iconPos.Enqueue(iconPos);
                        iconPos.y += heightGap;
                        stageData.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // �Ϲ� ����
                else
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        int randomValue = Random.Range(0, GetTotalProbability());
                        int sum = 0;
                        foreach (var kvp in probabilityMap)
                        {
                            sum += kvp.Value;
                            if (randomValue < sum)
                            {
                                stageData.iconPos.Enqueue(iconPos);
                                iconPos.y += heightGap;
                                stageData.iconTypes.Enqueue(kvp.Key);
                                switch(kvp.Key)
                                {
                                    case ICON.MONSTER:
                                        if(probabilityMap.ContainsKey(ICON.SHOP))
                                        {
                                            probabilityMap[ICON.SHOP] += 10;
                                        }
                                        break;

                                    case ICON.SHOP:
                                        currentShopCount += 1;
                                        probabilityMap[ICON.SHOP] = iconProbability.shopChance;
                                        if (currentShopCount == stageLevel.shopCount)
                                        {
                                            probabilityMap.Remove(ICON.SHOP);
                                        }
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            iconPos.x += widthGap;
        }
            stageData.isSave = true;
            DataManager.instance.stageData = stageData;
    }

    /**********************************************************
    * ���� �׸��⸦ ���� ��� ����
    ***********************************************************/
    private void MakeTreeData()
    {
        int preIconCount = 0;
        int iconCount;
        int countGap;

        for (int i = 0; i < stageData.lineCount; i++)
        {
            iconCount = stageData.iconCounts.Dequeue();

            for (int j = 0; j < iconCount; j++)
            {
                if( i == 0)
                {
                    preIconCount = iconCount;
                }
                else
                {
                    countGap = iconCount - preIconCount;
                }
            }
        }
    }

    /**********************************************************
    * �� ������ ���� ������ ����
    ***********************************************************/
    private void SetData()
    {
        currentStage = GameManager.instance.currentStage;
        currentStage = 3;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        mapWidth = stageLevel.mapWidth;
        mapHeight = stageLevel.mapHeight;

        // �ڵ� Add�� ���� ���߿�
        probabilityMap.Add(ICON.SHOP, iconProbability.shopChance);
        probabilityMap.Add(ICON.MONSTER, iconProbability.monsterChance);

        stageData.isSave = false;
        stageData.currentStage = currentStage;
        stageData.clearCount = 0;
        stageData.lineCount = stageLevel.lineCount;
        stageData.iconCounts.Clear();
        stageData.iconTypes.Clear();
    }

    /**********************************************************
    * ���� �������� ��ü Ȯ��
    ***********************************************************/
    private int GetTotalProbability()
    {
        int totalProbability = 0;
        foreach (var kvp in probabilityMap)
        {
            totalProbability += kvp.Value;
        }
        return totalProbability;
    }


    /**********************************************************
    * ���� �������� �� �� ���
    ***********************************************************/
    //private ICON PickIcon()
    //{
    //    int randomValue = Random.Range(0, 100);
    //    int sum = 0;
    //    foreach (var kvp in probabilityMap)
    //    {
    //        sum += kvp.Value;
    //        if (randomValue < sum)
    //        {
    //            return kvp.Key;
    //        }
    //    }
    //    throw new System.Exception($"{GetType()} - ���� ������ �� �޾ƿ�");
    //}


    // ��
}
