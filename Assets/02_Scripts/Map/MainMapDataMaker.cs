/**********************************************************
* ���� �� ���� �����͸� ������
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
            Debug.Log($"{GetType()} - ���ο� �� ������ ����");
            MakeMapData();
        }
    }


    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    * - ������ ó�� ������ ��, �� stage�� Ŭ���� ���� �� ���
    ***********************************************************/
    public void MakeMapData()
    {
        int iconCount;
        int iconType;
        int currentShopCount = 0;

        stageData.lineCount = stageLevel.lineCount;
        stageData.clearCount = 0;

        Debug.Log($"{GetType()} - ���� �� {stageData.lineCount}");
        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // ù��° ����
            if(i == 0) 
            {
                stageData.iconCounts.Enqueue(stageLevel.firstLine);

                for(int j = 0; j < stageLevel.firstLine; j++)
                {
                    stageData.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // ������ ����
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
                // ���� ����
                if (i == stageLevel.chestLine)
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageData.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // �Ϲ� ����
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
    * ���� �������� �� �� ���
    ***********************************************************/
    private void PickIcon()
    {

    }


    // ��
}
