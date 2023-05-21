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

        int iconCount = 0;

        int currentShopCount = 0;

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // 첫번째 라인
            if(i == 0) 
            {
                stageData.iconCounts.Enqueue(stageLevel.firstLine);

                for(int j = 0; j < stageLevel.firstLine; j++)
                {
                    stageData.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // 마지막 라인
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
                // 상자 라인
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageData.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // 일반 라인
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
    * 맵 생성을 위한 데이터 갱신
    ***********************************************************/
    private void SetData()
    {
        currentStage = GameManager.instance.currentStage;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

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
