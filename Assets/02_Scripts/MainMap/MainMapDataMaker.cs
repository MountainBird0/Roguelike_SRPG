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

    private MapData mapData;


    private int currentShopCount;
    private int currenteliteCount;

    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<IconType, int>();

        mapData = new MapData();
    }


    /**********************************************************
    * ���ο� �� ������ ���� ������ ���� �� DataMgr�� ����
    ***********************************************************/
    public void MakeMapData()
    {
        SetData();

        SetProbabilityDic();

        SetIconInfo();

        SetIconGrid();
        
        DataManager.instance.mapData = mapData;
        GameManager.instance.hasSaveData = true;
    }


    /**********************************************************
    * �� ������ ���� default������ �غ�
    ***********************************************************/
    private void SetData()
    {
        Random.InitState(DataManager.instance.gameInfo.seed);
        int currentStage = DataManager.instance.gameInfo.currentStage;

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
        for (int i = 0; i < stageLevel.firstLine; i++)
        {
            iconTypes.Add(IconType.MONSTER);
            iconCounts.Add(stageLevel.firstLine);
            mapData.iconStates.Add(IconState.ATTAINABLE);
        }

        // ù �����̶� ���������� ����
        for (int i = 1; i < stageLevel.lineCount - 1; i++)
        {
            int iconCount;
            if (i == stageLevel.chestLine - 1)
            {
                iconCount = Random.Range(minIconCount, maxIconCount);
                for (int j = 0; j < iconCount; j++)
                {
                    iconTypes.Add(IconType.CHEST);
                    iconCounts.Add(iconCount);
                    mapData.iconStates.Add(IconState.LOCKED);
                }
            }
            else
            {
                iconCount = Random.Range(minIconCount, maxIconCount);
                for (int j = 0; j < iconCount; j++)
                {
                    iconTypes.Add(GetRandomIcon());
                    iconCounts.Add(iconCount);
                    mapData.iconStates.Add(IconState.LOCKED);
                }
            }
        }

        for (int i = 0; i < stageLevel.lastLine; i++)
        {
            iconTypes.Add(IconType.BOSS);
            iconCounts.Add(stageLevel.lastLine);
        }
    }
    /**********************************************************
    * ���� ������ �޾ƿ���
    ***********************************************************/
    private IconType GetRandomIcon()
    {

        int randomValue = Random.Range(0, GetTotalProbability());

        int sum = 0;

        foreach (var kvp in probabilityMap)
        {
            sum += kvp.Value;

            if (randomValue < sum)
            {
                ProbabilityCheck(kvp.Key);
                return kvp.Key;
            }
        }
        throw new System.Exception("������ ��ã��");
    }


    /**********************************************************
    * ������ ��ġ ���� �ֱ�
    ***********************************************************/
    private void SetIconPos(List<int> iconCounts, List<Vector2> iconPos)
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
                ranVec.x = Random.Range(-0.5f, 0.5f);
                ranVec.y = Random.Range(-0.5f, 0.5f);
                iconPos.Add(pos + ranVec);
                pos.y += heightGap;
            }
            pos.x += widthGap;
        }
    }


    /**********************************************************
    * ������ ���� �ֱ�
    ***********************************************************/
    private void SetIconInfo()
    {
        float mapWidth = stageLevel.mapWidth;

        Vector2 iconPos = Vector2.zero;

        iconPos.x = -(mapWidth / 2); // �������� �ʱ� x��ǥ �� (���� �Ʒ�)
        float widthGap = mapWidth / (stageLevel.lineCount - 1);

        // �� ������ �� ����
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if (i == 0)
            {
                SetIcon(stageLevel.firstLine, ref iconPos, IconType.MONSTER, IconState.ATTAINABLE);
            }
            // ������ ����
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(stageLevel.lastLine, ref iconPos, IconType.BOSS, IconState.LOCKED);
            }
            else
            {
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    SetIcon(Random.Range(minIconCount, maxIconCount), ref iconPos, IconType.CHEST, IconState.LOCKED);
                }
                else
                {
                    SetIcon(Random.Range(minIconCount, maxIconCount), ref iconPos, IconState.LOCKED);
                }
            }

            iconPos.x += widthGap;
        }
    }


    /**********************************************************
    * ��� ���� �ֱ�
    ***********************************************************/
    private void SetIconGrid()
    {
        int maxIconPos = 0; // �������� �ִ�� �� �� �ִ� ������ ��ġ
        int gap = 0;        // �������ΰ� ��������� ������ �� ����
        int parIcon = 0;    // ����� �θ� ������
        int drawCount;      // �� �� ��������

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            if (i != 0)
            {
                gap = mapData.iconCounts[i - 1] - mapData.iconCounts[i];
            }

            for (int j = 0; j < mapData.iconCounts[i]; j++)
            {
                if (j == mapData.iconCounts[i] - 1)
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
            maxIconPos += mapData.iconCounts[i];
        }
    }


    /**********************************************************
    * ���� ������ ������ �ֱ�
    ***********************************************************/
    private void SetIcon(int iconCount, ref Vector2 pos, IconType icon, IconState iconState)
    {
        mapData.iconCounts.Add(iconCount);
        float mapHeight = stageLevel.mapHeight;

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            mapData.iconInfo.Add((icon, pos));
            mapData.iconStates.Add(iconState);
            float yRan = Random.Range(-0.5f, 0.5f);
            pos.y += (heightGap + yRan);
        }
    }
    /**********************************************************
    * ���� ������ ������ �ֱ�
    ***********************************************************/
    private void SetIcon(int iconCount, ref Vector2 pos, IconState iconState)
    {
        mapData.iconCounts.Add(iconCount);
        float mapHeight = stageLevel.mapHeight;

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            int randomValue = Random.Range(0, GetTotalProbability());

            int sum = 0;

            foreach (var kvp in probabilityMap)
            {
                sum += kvp.Value;

                if (randomValue < sum)
                {
                    mapData.iconInfo.Add((kvp.Key, pos));
                    mapData.iconStates.Add(iconState);
                    float yRan = Random.Range(-0.5f, 0.5f);
                    pos.y += (heightGap + yRan);
                    ProbabilityCheck(kvp.Key);
                    break;
                }
            }
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
                probabilityMap[IconType.SHOP] = iconProbability.shopChance;
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
            totalProbability += kvp.Value;
        }
        return totalProbability;
    }





    // ��
}
