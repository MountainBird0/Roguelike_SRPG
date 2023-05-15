/**********************************************************
* 메인 맵 관련 데이터를 생성함
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapDataMaker : MonoBehaviour
{
    public StageLevelData stageLevel;

    public int currentStage;

    private void Awake()
    {
        stageLevel = new StageLevelData();
    }

    private void Start()
    {
        currentStage = GameManager.instance.currentStage;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
    }


    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    * - 게임을 처음 시작할 때, 한 stage를 클리어 했을 때 사용
    ***********************************************************/
    public void MakeMapData()
    {
        //int iconCount


        for(int i = 0; i < stageLevel.lineNum; i++)
        {
            //for(int j = 0; j < )

        }


    }



}
