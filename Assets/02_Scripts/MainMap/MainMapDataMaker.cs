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
    private Dictionary<Icon, int> probabilityMap;

    private MapData mapData;

    private int currentShopCount;


    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<Icon, int>();

        mapData = new MapData();
    }


    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    ***********************************************************/
    public void MakeMapData()
    {
        SetData();

        float mapWidth = stageLevel.mapWidth;
        float mapHeight = stageLevel.mapHeight;

        Vector2 iconPos = Vector2.zero;

        iconPos.x = -(mapWidth / 2); // �������� �ʱ� x��ǥ �� (���� �Ʒ�)
        float widthGap = mapWidth / (stageLevel.lineCount - 1);

        // �� ������ �� ����
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if (i == 0)
            {
                SetIcon(mapHeight, stageLevel.firstLine, Icon.MONSTER, ref iconPos);
            }
            // ������ ����
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(mapHeight, stageLevel.lastLine, Icon.BOSS, ref iconPos);
            }
            else
            {
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    SetIcon(mapHeight, Random.Range(2, 6), Icon.CHEST, ref iconPos);
                }
                else
                {
                    SetIcon(mapHeight, Random.Range(2, 6), ref iconPos);
                }
            }
            iconPos.x += widthGap;
        }

        SetIconGrid();
        
        DataManager.instance.mapData = mapData;
        GameManager.instance.hasSaveData = true;
    }


    /**********************************************************
    * ���� ������ ������ �ֱ�
    ***********************************************************/
    public void SetIcon(float mapHeight, int iconCount, Icon icon, ref Vector2 pos)
    {
        mapData.iconCounts.Add(iconCount);

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            mapData.iconState.Add((icon, pos, IconState.LOCKED));
            pos.y += heightGap;
        }
    }
    /**********************************************************
    * ���� ������ ������ �ֱ�
    ***********************************************************/
    public void SetIcon(float mapHeight, int iconCount, ref Vector2 pos)
    {
        mapData.iconCounts.Add(iconCount);

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
                    mapData.iconState.Add((kvp.Key, pos, IconState.LOCKED));
                    pos.y += heightGap;
                    ProbabilityCheck(kvp.Key);
                    break;
                }
            }
        }
    }


    /**********************************************************
    * ���� ������ dic����
    ***********************************************************/
    private void ProbabilityCheck(Icon icon)
    {
        switch (icon)
        {
            case Icon.MONSTER:
                if (probabilityMap.ContainsKey(Icon.SHOP))
                {
                    probabilityMap[Icon.SHOP] += 10;
                }
                break;

            case Icon.SHOP:
                currentShopCount += 1;
                probabilityMap[Icon.SHOP] = iconProbability.shopChance;
                if (currentShopCount == stageLevel.shopCount)
                {
                    probabilityMap.Remove(Icon.SHOP);
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

    /**********************************************************
    * �� ������ ���� default������ �غ�
    ***********************************************************/
    private void SetData()
    {
        Random.InitState(DataManager.instance.gameInfo.seed);
        int currentStage = DataManager.instance.gameInfo.currentStage;

        currentStage = 3;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        mapData.lineCount = stageLevel.lineCount;

        // dic ����
        probabilityMap.Clear();
        currentShopCount = 0;

        probabilityMap.Add(Icon.SHOP, iconProbability.shopChance);
        probabilityMap.Add(Icon.MONSTER, iconProbability.monsterChance);
    }


    /**********************************************************
    * ������� �ֱ�
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



    // ��
}
