/**********************************************************
* ���� �� ���� �����͸� ������
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapDataMaker)]
public class MainMapDataMaker : MonoBehaviour
{
    private const int minIconCount = 2;
    private const int maxIconCount = 6;

    private StageLevelData stageLevel;
    private IconProbabilityData iconProbability;
    private Dictionary<IconType, int> probabilityMap;

    private MapInfo mapData;


    private int currentShopCount;
    private int currenteliteCount;

    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<IconType, int>();

        mapData = new MapInfo();
    }

    /**********************************************************
    * ���ο� �� ������ ���� ������ ���� �� DataMgr�� ����
    ***********************************************************/
    public void MakeMapData()
    {
        List<int> iconCounts = new List<int>();
        List<IconType> iconTypes = new List<IconType>();
        List<Vector2> iconPositions = new List<Vector2>();

        SetData();

        SetProbabilityDic();

        SetIconType(iconCounts, iconTypes);
        SetIconPos(iconCounts, iconPositions);
        for (int i = 0; i < iconTypes.Count; i++)
        {
            mapData.iconInfo.Add((iconTypes[i], iconPositions[i]));
        }


        SetIconGrid(iconCounts);

        DataManager.instance.mapInfo = mapData;
        GameManager.instance.hasSaveData = true;
    }

    /**********************************************************
    * �� ������ ���� default������ �غ�
    ***********************************************************/
    private void SetData()
    {
        Random.InitState(DataManager.instance.gameInfo.seed);
        int currentStage = DataManager.instance.gameInfo.currentStage;
        // currentStage = 3;

        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];
    }

    /**********************************************************
    * ���� ������ ������ ���� dic ����
    ***********************************************************/
    private void SetProbabilityDic()
    {
        currentShopCount = 0;
        currenteliteCount = 0;

        probabilityMap.Clear();

        probabilityMap.Add(IconType.SHOP, iconProbability.shopChance);
        probabilityMap.Add(IconType.ELITE, iconProbability.eliteChance);
        probabilityMap.Add(IconType.MONSTER, iconProbability.monsterChance);
    }

    /**********************************************************
    * ������ ���� ���� �ֱ�
    ***********************************************************/
    private void SetIconType(List<int> iconCounts, List<IconType> iconTypes)
    {
        iconCounts.Add(stageLevel.firstCount);
        for (int i = 0; i < stageLevel.firstCount; i++)
        {
            iconTypes.Add(IconType.MONSTER);         
            mapData.iconStates.Add(IconState.ATTAINABLE);
        }

        // ù �����̶� ���������� ����
        for (int i = 1; i < stageLevel.lineCount - 1; i++)
        {
            int iconCount;
            if (i == stageLevel.chestLine - 1)
            {
                iconCount = Random.Range(minIconCount, maxIconCount);
                iconCounts.Add(iconCount);
                for (int j = 0; j < iconCount; j++)
                {
                    iconTypes.Add(IconType.CHEST);                    
                    mapData.iconStates.Add(IconState.LOCKED);
                }
            }
            else
            {
                iconCount = Random.Range(minIconCount, maxIconCount);
                iconCounts.Add(iconCount);
                for (int j = 0; j < iconCount; j++)
                {
                    iconTypes.Add(GetRandomIcon());                  
                    mapData.iconStates.Add(IconState.LOCKED);
                }
            }
        }

        iconCounts.Add(stageLevel.lastCount);
        for (int i = 0; i < stageLevel.lastCount; i++)
        {
            iconTypes.Add(IconType.BOSS);          
            mapData.iconStates.Add(IconState.LOCKED);
        }
    }
    /**********************************************************
    * ���� ������ �޾ƿ���
    ***********************************************************/
    private IconType GetRandomIcon()
    {

        int randomValue = Random.Range(0, GetTotalProbability());
        //Debug.Log($"{GetType()} - ������ - {randomValue}");

        int sum = 0;

        foreach (var kvp in probabilityMap)
        {
            sum += kvp.Value;
            //Debug.Log($"{GetType()} - sum({sum})�� �� ����{kvp.Value}");

            if (randomValue < sum)
            {
               // Debug.Log($"{GetType()} - �ɸ��� - {kvp.Key}");

                ProbabilityCheck(kvp.Key);
                return kvp.Key;
            }
        }
        throw new System.Exception("������ ��ã��");
    }

    /**********************************************************
    * ������ ��ġ ���� �ֱ�
    ***********************************************************/
    private void SetIconPos(List<int> iconCounts, List<Vector2> iconPositions)
    {
        float mapWidth = stageLevel.mapWidth;
        float mapHeight = stageLevel.mapHeight;

        Vector2 pos = Vector2.zero;
        Vector2 ranVec = Vector2.zero;

        pos.x = -(mapWidth / 2); // �������� �ʱ� x��ǥ �� (���� �Ʒ�)
        float widthGap = mapWidth / (stageLevel.lineCount - 1);

        for(int i = 0; i < iconCounts.Count; i++)
        {
            float heightGap = mapHeight / (iconCounts[i] + 1);
            pos.y = -(mapHeight / 2) + heightGap;
            for (int j = 0; j < iconCounts[i]; j++)
            {
                ranVec.x = Random.Range(-0.2f, 0.2f);
                ranVec.y = Random.Range(-0.2f, 0.2f);
                iconPositions.Add(pos + ranVec);
                pos.y += heightGap;
            }
            pos.x += widthGap;
        }
    }

    /**********************************************************
    * ��� ���� �ֱ�
    ***********************************************************/
    private void SetIconGrid(List<int> iconCounts)
    {
        int maxIconPos = 0; // �������� �ִ�� �� �� �ִ� ������ ��ġ
        int gap = 0;        // �������ΰ� ��������� ������ �� ����
        int parIcon = 0;    // ����� �θ� ������
        int drawCount;      // �� �� ��������

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            if (i != 0)
            {
                gap = iconCounts[i - 1] - iconCounts[i];
            }

            for (int j = 0; j < iconCounts[i]; j++)
            {
                if (j == iconCounts[i] - 1)
                {
                    mapData.nodeDatas.Add((parIcon, maxIconPos - parIcon + 1));
                }
                else
                {
                    drawCount = Random.Range(1, gap + 1);
                    if (drawCount < 1)
                    {
                        drawCount = 1;
                    }
                    mapData.nodeDatas.Add((parIcon, drawCount));
                    parIcon += drawCount + Random.Range(-1, 1);
                }

                if (parIcon > maxIconPos)
                {
                    parIcon = maxIconPos;
                }
            }
            parIcon = maxIconPos + 1;
            maxIconPos += iconCounts[i];
        }
    }

    /**********************************************************
    * ���� ������ dic����
    ***********************************************************/
    private void ProbabilityCheck(IconType icon)
    {
        switch (icon)
        {
            case IconType.MONSTER:
                if (probabilityMap.ContainsKey(IconType.SHOP))
                {
                    probabilityMap[IconType.SHOP] += 10;
                }
                if (probabilityMap.ContainsKey(IconType.ELITE))
                {
                    probabilityMap[IconType.ELITE] += 10;
                }
                break;

            case IconType.SHOP:
                currentShopCount += 1;
                probabilityMap[IconType.SHOP] = iconProbability.shopChance;
                if (currentShopCount == stageLevel.shopCount)
                {
                    probabilityMap.Remove(IconType.SHOP);
                }
                break;
            case IconType.ELITE:
                currenteliteCount += 1;
                probabilityMap[IconType.ELITE] = iconProbability.eliteChance;
                if (currenteliteCount == stageLevel.eliteCount)
                {
                    probabilityMap.Remove(IconType.ELITE);
                }
                break;
        }
    }

    /**********************************************************
    * ���� �������� ��ü Ȯ�� ���ϱ�
    ***********************************************************/
    private int GetTotalProbability()
    {
        int totalProbability = 0;

        foreach (var kvp in probabilityMap)
        {
            // Debug.Log($"{GetType()} - ��Ȯ�� - {kvp.Key}, {kvp.Value}");

            totalProbability += kvp.Value;
        }
        // Debug.Log($"{GetType()} - ���� - {totalProbability}");
        return totalProbability;
    }





    // ��
}
