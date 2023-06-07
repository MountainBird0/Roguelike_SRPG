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
    private Dictionary<ICON, int> probabilityMap;

    private GameInfo gameInfo;
    private MapData mapData;

    private int currentStage;

    private int currentShopCount;


    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<ICON, int>();

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

        AddProbabilityDic();

        // �� ������ �� ����
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if (i == 0)
            {
                SetIcon(mapHeight, stageLevel.firstLine, ICON.MONSTER, ref iconPos);
            }
            // ������ ����
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(mapHeight, stageLevel.lastLine, ICON.BOSS, ref iconPos);
            }
            else
            {
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    SetIcon(mapHeight, Random.Range(2, 6), ICON.CHEST, ref iconPos);
                }
                else
                {
                    SetIcon(mapHeight, Random.Range(2, 6), ref iconPos);
                }
            }
            iconPos.x += widthGap;
        }

        SetIconGrid();
        mapData.lineCount = stageLevel.lineCount;
        DataManager.instance.mapData = mapData;
    }


    /**********************************************************
    * ���� ������ ������ �ֱ�
    ***********************************************************/
    public void SetIcon(float mapHeight, int iconCount, ICON icon, ref Vector2 pos)
    {
        mapData.iconCounts.Add(iconCount);

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            mapData.iconState.Add((icon, pos));
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
                    mapData.iconState.Add((kvp.Key, pos));
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
    private void ProbabilityCheck(ICON icon)
    {
        switch (icon)
        {
            case ICON.MONSTER:
                if (probabilityMap.ContainsKey(ICON.SHOP))
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
    }


    /**********************************************************
    * ���� ������ dic �߰�
    ***********************************************************/
    private void AddProbabilityDic()
    {
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        probabilityMap.Clear();
        currentShopCount = 0;

        probabilityMap.Add(ICON.SHOP, iconProbability.shopChance);
        probabilityMap.Add(ICON.MONSTER, iconProbability.monsterChance);
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


    /**********************************************************
    * �� ������ ���� default������ �غ�
    ***********************************************************/
    private void SetData()
    {
        // �õ� ����
        int seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        Random.InitState(seed);
        DataManager.instance.gameInfo.seed = seed;

        currentStage = DataManager.instance.mapData.currentStage;
        currentStage = 1;

        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];     
    }

    // ��
}
