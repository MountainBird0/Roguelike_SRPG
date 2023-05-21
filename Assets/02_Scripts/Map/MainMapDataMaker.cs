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



    public int currentStage;

    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        stageData = new StageData()
        {
            iconCounts = new Queue<int>(),
            iconTypes = new Queue<ICON>()
        };
        probabilityMap = new Dictionary<ICON, int>();
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

        int iconCount = 0;

        int currentShopCount = 0;

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if(i == 0) 
            {
                stageData.iconCounts.Enqueue(stageLevel.firstLine);

                for(int j = 0; j < stageLevel.firstLine; j++)
                {
                    stageData.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // ������ ����
            else if (i == (stageLevel.lineCount - 1))
            {
                stageData.iconCounts.Enqueue(stageLevel.lastLine);

                for (int j = 0; j < stageLevel.lastLine; j++)
                {
                    stageData.iconTypes.Enqueue(ICON.BOSS);
                }
            }
            else
            {
                iconCount = Random.Range(2, 5);
                stageData.iconCounts.Enqueue(iconCount);
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageData.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // �Ϲ� ����
                else
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        int totalProbability = 0;
                        foreach(var kvp in probabilityMap)
                        {
                            totalProbability += kvp.Value;
                        }

                        int randomValue = Random.Range(0, totalProbability);
                        int sum = 0;
                        foreach (var kvp in probabilityMap)
                        {
                            sum += kvp.Value;
                            if (randomValue < sum)
                            {
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
        }
            stageData.isSave = true;
            DataManager.instance.stageData = stageData;
    }

    /**********************************************************
    * �� ������ ���� ������ ����
    ***********************************************************/
    private void SetData()
    {
        currentStage = GameManager.instance.currentStage;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

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
