/**********************************************************
* 메인 맵 관련 데이터를 생성함
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapDataMaker)]
public class MainMapDataMaker : MonoBehaviour
{
    private StageLevelData stageLevel;
    private IconProbabilityData iconProbability;
    private Dictionary<IconType, int> probabilityMap;

    private MapData mapData;

    private int currentShopCount;

    private const int minIconCount = 2;
    private const int maxIconCount = 6;


    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<IconType, int>();

        mapData = new MapData();
    }


    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    ***********************************************************/
    public void MakeMapData()
    {
        SetData();

        float mapWidth = stageLevel.mapWidth;
        float mapHeight = stageLevel.mapHeight;

        Vector2 iconPos = Vector2.zero;

        iconPos.x = -(mapWidth / 2); // 아이콘의 초기 x좌표 값 (가장 아래)
        float widthGap = mapWidth / (stageLevel.lineCount - 1);

        // 한 레벨의 각 라인
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // 첫번째 라인
            if (i == 0)
            {
                SetIcon(mapHeight, stageLevel.firstLine, IconType.MONSTER, ref iconPos);
            }
            // 마지막 라인
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(mapHeight, stageLevel.lastLine, IconType.BOSS, ref iconPos);
            }
            else
            {
                // 상자 라인
                if (i == (stageLevel.chestLine - 1))
                {
                    SetIcon(mapHeight, Random.Range(minIconCount, maxIconCount), IconType.CHEST, ref iconPos);
                }
                else
                {
                    SetIcon(mapHeight, Random.Range(minIconCount, maxIconCount), ref iconPos);
                }
            }
            iconPos.x += widthGap;
        }

        SetIconGrid();
        
        DataManager.instance.mapData = mapData;
        GameManager.instance.hasSaveData = true;
    }


    /**********************************************************
    * 고정 아이콘 데이터 넣기
    ***********************************************************/
    public void SetIcon(float mapHeight, int iconCount, IconType icon, ref Vector2 pos)
    {
        mapData.iconCounts.Add(iconCount);

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            mapData.iconInfo.Add((icon, pos));
            pos.y += heightGap;
        }
    }
    /**********************************************************
    * 랜덤 아이콘 데이터 넣기
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
                    mapData.iconInfo.Add((kvp.Key, pos));
                    pos.y += heightGap;
                    ProbabilityCheck(kvp.Key);
                    break;
                }
            }
        }
    }


    /**********************************************************
    * 랜덤 아이콘 dic관리
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
                break;

            case IconType.SHOP:
                currentShopCount += 1;
                probabilityMap[IconType.SHOP] = iconProbability.shopChance;
                if (currentShopCount == stageLevel.shopCount)
                {
                    probabilityMap.Remove(IconType.SHOP);
                }
                break;
        }
    }


    /**********************************************************
    * 랜덤 아이콘의 전체 확률 구하기
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
    * 맵 생성을 위한 default데이터 준비
    ***********************************************************/
    private void SetData()
    {
        Random.InitState(DataManager.instance.gameInfo.seed);
        int currentStage = DataManager.instance.gameInfo.currentStage;

        currentStage = 3;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        mapData.lineCount = stageLevel.lineCount;

        // dic 관련
        probabilityMap.Clear();
        currentShopCount = 0;

        probabilityMap.Add(IconType.SHOP, iconProbability.shopChance);
        probabilityMap.Add(IconType.MONSTER, iconProbability.monsterChance);
    }


    /**********************************************************
    * 노드정보 넣기
    ***********************************************************/
    private void SetIconGrid()
    {
        int maxIconPos = 0; // 이전라인 최대로 들어갈 수 있는 아이콘 위치
        int gap = 0;        // 이전라인과 현재라인의 아이콘 수 차이
        int parIcon = 0;    // 연결될 부모 아이콘
        int drawCount;      // 몇 번 연결할지

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



    // 끝
}
