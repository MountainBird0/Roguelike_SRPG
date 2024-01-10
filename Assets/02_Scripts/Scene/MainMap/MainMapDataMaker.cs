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
    * 새로운 맵 생성을 위한 데이터 만든 후 DataMgr에 저장
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
    * 맵 생성을 위한 default데이터 준비
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
        iconCounts.Add(stageLevel.firstCount);
        for (int i = 0; i < stageLevel.firstCount; i++)
        {
            iconTypes.Add(IconType.MONSTER);         
            mapData.iconStates.Add(IconState.ATTAINABLE);
        }

        // 첫 라인이랑 마지막라인 제외
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
    * 랜덤 아이콘 받아오기
    ***********************************************************/
    private IconType GetRandomIcon()
    {

        int randomValue = Random.Range(0, GetTotalProbability());
        //Debug.Log($"{GetType()} - 랜덤값 - {randomValue}");

        int sum = 0;

        foreach (var kvp in probabilityMap)
        {
            sum += kvp.Value;
            //Debug.Log($"{GetType()} - sum({sum})에 값 더함{kvp.Value}");

            if (randomValue < sum)
            {
               // Debug.Log($"{GetType()} - 걸린거 - {kvp.Key}");

                ProbabilityCheck(kvp.Key);
                return kvp.Key;
            }
        }
        throw new System.Exception("아이콘 못찾음");
    }

    /**********************************************************
    * 아이콘 위치 정보 넣기
    ***********************************************************/
    private void SetIconPos(List<int> iconCounts, List<Vector2> iconPositions)
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
                ranVec.x = Random.Range(-0.2f, 0.2f);
                ranVec.y = Random.Range(-0.2f, 0.2f);
                iconPositions.Add(pos + ranVec);
                pos.y += heightGap;
            }
            pos.x += widthGap;
        }
    }

    /**********************************************************
    * 노드 정보 넣기
    ***********************************************************/
    private void SetIconGrid(List<int> iconCounts)
    {
        int maxIconPos = 0; // 이전라인 최대로 들어갈 수 있는 아이콘 위치
        int gap = 0;        // 이전라인과 현재라인의 아이콘 수 차이
        int parIcon = 0;    // 연결될 부모 아이콘
        int drawCount;      // 몇 번 연결할지

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
                probabilityMap[IconType.ELITE] = iconProbability.eliteChance;
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
            // Debug.Log($"{GetType()} - 총확률 - {kvp.Key}, {kvp.Value}");

            totalProbability += kvp.Value;
        }
        // Debug.Log($"{GetType()} - 총합 - {totalProbability}");
        return totalProbability;
    }





    // 끝
}
