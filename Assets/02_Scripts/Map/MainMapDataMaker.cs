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
        // 새로하기 의미
        if (!stageData.isSave)
        {
            Debug.Log($"{GetType()} - 새로운 맵 데이터 생성");
            MakeMapData();
        }
    }


    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    * - 게임을 처음 시작할 때, 한 stage를 클리어 했을 때 사용
    * - 상점 가중확률 추가 필요
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
            // 첫번째 라인
            if (i == 0) 
            {
                iconTrees = new List<IconTree>(stageLevel.firstLine);

                stageData.iconCounts.Enqueue(stageLevel.firstLine);

                heightGap = mapHeight / (stageLevel.firstLine + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;

                for (int j = 0; j < stageLevel.firstLine; j++)
                {
                    //노드 관련?
                    if( j == 0)
                    {

                        // 다음 j = 0과 연결
                    }
                    else if(j == stageLevel.firstLine - 1)
                    {
                        // 다음 j = stageLevel.firstLine - 1과 연결
                    }
                    else
                    {

                    }



                    stageData.iconPos.Enqueue(iconPos);
                    iconPos.y += heightGap;
                    stageData.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // 마지막 라인
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
                // 상자 라인
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageData.iconPos.Enqueue(iconPos);
                        iconPos.y += heightGap;
                        stageData.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // 일반 라인
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
    * 라인 그리기를 위한 노드 생성
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
    * 맵 생성을 위한 데이터 갱신
    ***********************************************************/
    private void SetData()
    {
        currentStage = GameManager.instance.currentStage;
        currentStage = 3;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        mapWidth = stageLevel.mapWidth;
        mapHeight = stageLevel.mapHeight;

        // 자동 Add로 변경 나중에
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
    * 랜덤 아이콘의 전체 확률
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
    * 랜덤 아이콘을 고를 때 사용
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
    //    throw new System.Exception($"{GetType()} - 랜덤 아이콘 못 받아옴");
    //}


    // 끝
}
