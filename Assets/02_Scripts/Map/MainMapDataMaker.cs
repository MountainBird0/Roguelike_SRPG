/**********************************************************
* ���� �� ���� �����͸� ������
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
    * ���ο� �� ������ ���� ������ ����
    * - ������ ó�� ������ ��, �� stage�� Ŭ���� ���� �� ���
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
