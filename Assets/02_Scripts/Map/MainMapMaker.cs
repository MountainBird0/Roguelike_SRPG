/******************************************************************************
* MainMap ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapMaker : MonoBehaviour
{
    public Transform map;

    [Space(10f)]
    //public float lineNum; // ������ ���� ��

    [Header("[ Icon ]")] // �����ܵ�
    public GameObject[] icons;

    public GameObject Monster;
    public GameObject Boss;
    public GameObject Shop;
    public GameObject Chest;

    // �� ����
    const int MAP_WIDTH = 14;
    const int MAP_HEIGHT = 6;

    private float gapWidth = 0;
    private float gapHeight = 0;




    private GameObject icon;

    private Vector2 moveVec = Vector2.zero;



    private Dictionary<string, StageLevelData> StageLevels;
    private StageData stageData;


    private void Awake()
    {
        StageLevels = new Dictionary<string, StageLevelData>();
        stageData = new StageData()
        {
            iconPerLines = new Queue<int>(),
            iconIndexs = new Queue<int>(),
        };
    }

    public void Start()
    {
        // StageData���� �޾ƿ��� �õ� ��
        stageData = DataManager.instance.stageData;
        Debug.Log($"{GetType()} - stagedata �� Ȯ�� : {stageData}");
        Debug.Log($"{GetType()} - stagedata �� Ȯ�� : {stageData.lineNum}");

        //if (stageData == null)
        //{

        //}

    }

    /**********************************************************
    * ������ ������� �� ����
    ***********************************************************/
    private void MakeMap()
    {
        int iconPerLine;
        int clearLine = stageData.clearLine;
        int iconIndex;
     
        GameObject icon;
        Vector2 iconPos = Vector2.zero;

        for (int i = 0; i < stageData.lineNum; i++)
        {
            iconPerLine = stageData.iconPerLines.Dequeue();
            for(int j = 0; j < iconPerLine; j++)
            {
                // ��ġ���

                iconIndex = stageData.iconIndexs.Dequeue();
                icon = Instantiate(icons[iconIndex], map);
                icon.transform.position = iconPos;
                if(i <= clearLine)
                {
                    icon.SetActive(false);
                }
            }
        }
    }

    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    ***********************************************************/
    public void MakeNewData()
    {
        int currentStage = GameManager.instance.currentStage;

        StageLevels = DataManager.instance.StageLevels;
        StageLevelData sld;

        if (StageLevels.TryGetValue(currentStage.ToString(), out sld))
        {
            for (int i = 0; i < sld.lineNum; i++)
            {
                if (i == 0)
                {
                    // ù ��
                }
                else if (i == sld.lineNum - 1)
                {
                    // ������ ��
                }
                else if (i == sld.chestLine)
                {
                    // ���� ��
                }
                else
                {
                    // ������
                }

            }

        }


        DataManager.instance.stageData = stageData; // DataManager�� ����
    }





    /**********************************************************
    * �ҷ��� ������ ����
    * ������ -7 ~ 7 // -3 ~ 3
    ***********************************************************/
    public void MakeMap(float lineNum)
    {
        gapWidth = MAP_WIDTH / (lineNum - 1);
        moveVec.x = -(MAP_WIDTH / 2);

        // ���� ����
        for (int i = 0; i < lineNum; i++)
        {
            Debug.Log($"{GetType()} - {i}, line : {lineNum - 1}");
            // ù��° ����
            if (i.Equals(0))
            {
                StartLine();
            }
            // ������ ����
            else if (i == lineNum - 1)
            {
                LastLine();
            }
            // �������� ����

            //�Ϲ� ����
            else
            {
                Debug.Log($"{GetType()} - �ٸ� ����");

                icon = Instantiate(Monster, map);
                moveVec.x += gapWidth;
                icon.transform.position = moveVec;
            }
        }
    }

    private void StartLine()
    {
        Debug.Log($"{GetType()} - ù ����");

        icon = Instantiate(Monster, map);
        icon.transform.position = moveVec;
    }
    private void LastLine()
    {
        Debug.Log($"{GetType()} - ������ ����");

        moveVec.x += gapWidth;
        icon = Instantiate(Boss, map);
        icon.transform.position = moveVec;
    }



    /**********************************************************
    * �ҷ��� ������ ����
    ***********************************************************/





    // test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - 1�� ���� �� ����");
            MakeMap();
            //MakeMap(lineNum);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"{GetType()} - 2�� ���� �����غ���");



            GameManager.instance.SaveGame();
        }


        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Debug.Log($"{GetType()} - 2�� ���� �����غ���");

        //stageDate.stageNum = 1;
        //stageDates.Add(Data);

        //stageDate.stageNum = 2;
        //stageDates.Add(Data);





        //    DataManager.instance.mainMapData = mainMapData;

        //    GameManager.instance.SaveGame();
        //}
    }

}
