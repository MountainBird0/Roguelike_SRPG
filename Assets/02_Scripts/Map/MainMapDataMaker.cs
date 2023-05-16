/**********************************************************
* 메인 맵 관련 데이터를 생성함
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapDataMaker : MonoBehaviour
{
    private StageLevelData stageLevel;
    private StageData stageData;

    public int currentStage;

    private void Awake()
    {
        stageLevel = new StageLevelData();
        stageData = new StageData()
        {
            iconCounts = new Queue<int>(),
            iconTypes = new Queue<ICON>()
        };
    }

    private void Start()
    {
        currentStage = GameManager.instance.currentStage;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];

        if(!stageData.isSave)
        {
            Debug.Log($"{GetType()} - 새로운 맵 데이터 생성");
            MakeMapData();
        }
    }


    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    * - 게임을 처음 시작할 때, 한 stage를 클리어 했을 때 사용
    ***********************************************************/
    public void MakeMapData()
    {
        int iconCount;
        int iconType;
        int currentShopCount = 0;

        stageData.lineCount = stageLevel.lineCount;
        stageData.clearCount = 0;

        Debug.Log($"{GetType()} - 라인 수 {stageData.lineCount}");
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

                for (int j = 0; j < stageLevel.firstLine; j++)
                {
                    stageData.iconTypes.Enqueue(ICON.BOSS);
                }
            }
            else
            {
                iconCount = Random.Range(0, 5);
                stageData.iconCounts.Enqueue(iconCount);
                // 상자 라인
                if (i == stageLevel.chestLine)
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
                        if (currentShopCount >= stageLevel.shopCount)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }

        }
            stageData.isSave = true;
            DataManager.instance.stageData = stageData;
    }

    /**********************************************************
    * 랜덤 아이콘을 고를 때 사용
    ***********************************************************/
    private void PickIcon()
    {

    }


    // 끝
}
