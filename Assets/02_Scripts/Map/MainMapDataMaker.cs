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

        AddProbabilityDic();

        // 한 레벨의 각 라인
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // 첫번째 라인
            if (i == 0)
            {
                SetIcon(mapHeight, stageLevel.firstLine, ICON.MONSTER, ref iconPos);
            }
            // 마지막 라인
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(mapHeight, stageLevel.lastLine, ICON.BOSS, ref iconPos);
            }
            else
            {
                // 상자 라인
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
    * 고정 아이콘 데이터 넣기
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
                    mapData.iconState.Add((kvp.Key, pos));
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
    * 랜덤 아이콘 dic 추가
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


    /**********************************************************
    * 맵 생성을 위한 default데이터 준비
    ***********************************************************/
    private void SetData()
    {
        // 시드 생성
        int seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        Random.InitState(seed);
        DataManager.instance.gameInfo.seed = seed;

        currentStage = DataManager.instance.mapData.currentStage;
        currentStage = 1;

        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];     
    }

    // 끝
}
