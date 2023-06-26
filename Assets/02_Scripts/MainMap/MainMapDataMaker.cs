/**********************************************************
* 메인 맵 관련 데이터를 생성함
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
    * 새로운 맵 생성을 위한 데이터 만든 후 DataMgr에 저장
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
    * 맵 생성을 위한 default데이터 준비
    ***********************************************************/
    private void SetData()
    {
        Random.InitState(DataManager.instance.gameInfo.seed);
        int currentStage = DataManager.instance.gameInfo.currentStage;

        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];
    }


    /**********************************************************
    * 랜덤 아이콘 생성을 위한 dic 세팅
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
    * 아이콘 종류 정보 넣기
    ***********************************************************/
    private void SetIconType(List<int> iconCounts, List<IconType> iconTypes)
    {
        for (int i = 0; i < stageLevel.firstLine; i++)
        {
            iconTypes.Add(IconType.MONSTER);
            iconCounts.Add(stageLevel.firstLine);
            mapData.iconStates.Add(IconState.ATTAINABLE);
        }

        // 첫 라인이랑 마지막라인 제외
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
    * 랜덤 아이콘 받아오기
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
        throw new System.Exception("아이콘 못찾음");
    }


    /**********************************************************
    * 아이콘 위치 정보 넣기
    ***********************************************************/
    private void SetIconPos(List<int> iconCounts, List<Vector2> iconPos)
    {
        float mapWidth = stageLevel.mapWidth;
        float mapHeight = stageLevel.mapHeight;

        Vector2 pos = Vector2.zero;
        Vector2 ranVec = Vector2.zero;

        pos.x = -(mapWidth / 2); // 아이콘의 초기 x좌표 값 (가장 아래)
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
    * 아이콘 정보 넣기
    ***********************************************************/
    private void SetIconInfo()
    {
        float mapWidth = stageLevel.mapWidth;

        Vector2 iconPos = Vector2.zero;

        iconPos.x = -(mapWidth / 2); // 아이콘의 초기 x좌표 값 (가장 아래)
        float widthGap = mapWidth / (stageLevel.lineCount - 1);

        // 한 레벨의 각 라인
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // 첫번째 라인
            if (i == 0)
            {
                SetIcon(stageLevel.firstLine, ref iconPos, IconType.MONSTER, IconState.ATTAINABLE);
            }
            // 마지막 라인
            else if (i == (stageLevel.lineCount - 1))
            {
                SetIcon(stageLevel.lastLine, ref iconPos, IconType.BOSS, IconState.LOCKED);
            }
            else
            {
                // 상자 라인
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
    * 노드 정보 넣기
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
    * 고정 아이콘 데이터 넣기
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
    * 랜덤 아이콘 데이터 넣기
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





    // 끝
}
