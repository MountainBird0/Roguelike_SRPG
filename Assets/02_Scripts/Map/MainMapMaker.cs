/******************************************************************************
* MainMap 생성
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapMaker : MonoBehaviour
{
    public Transform map;

    [Space(10f)]
    //public float lineNum; // 생성할 라인 수

    [Header("[ Icon ]")] // 아이콘들
    public GameObject[] icons;

    public GameObject Monster;
    public GameObject Boss;
    public GameObject Shop;
    public GameObject Chest;

    // 맵 관련
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
        // StageData정보 받아오기 시도 후
        stageData = DataManager.instance.stageData;
        Debug.Log($"{GetType()} - stagedata 값 확인 : {stageData}");
        Debug.Log($"{GetType()} - stagedata 값 확인 : {stageData.lineNum}");

        //if (stageData == null)
        //{

        //}

    }

    /**********************************************************
    * 데이터 기반으로 맵 생성
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
                // 위치계산

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
    * 새로운 맵 생성을 위한 데이터 만듬
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
                    // 첫 줄
                }
                else if (i == sld.lineNum - 1)
                {
                    // 마지막 줄
                }
                else if (i == sld.chestLine)
                {
                    // 상자 줄
                }
                else
                {
                    // 나머지
                }

            }

        }


        DataManager.instance.stageData = stageData; // DataManager에 저장
    }





    /**********************************************************
    * 불러온 맵으로 생성
    * 사이즈 -7 ~ 7 // -3 ~ 3
    ***********************************************************/
    public void MakeMap(float lineNum)
    {
        gapWidth = MAP_WIDTH / (lineNum - 1);
        moveVec.x = -(MAP_WIDTH / 2);

        // 라인 생성
        for (int i = 0; i < lineNum; i++)
        {
            Debug.Log($"{GetType()} - {i}, line : {lineNum - 1}");
            // 첫번째 라인
            if (i.Equals(0))
            {
                StartLine();
            }
            // 마지막 라인
            else if (i == lineNum - 1)
            {
                LastLine();
            }
            // 보물상자 라인

            //일반 라인
            else
            {
                Debug.Log($"{GetType()} - 다른 라인");

                icon = Instantiate(Monster, map);
                moveVec.x += gapWidth;
                icon.transform.position = moveVec;
            }
        }
    }

    private void StartLine()
    {
        Debug.Log($"{GetType()} - 첫 라인");

        icon = Instantiate(Monster, map);
        icon.transform.position = moveVec;
    }
    private void LastLine()
    {
        Debug.Log($"{GetType()} - 마지막 라인");

        moveVec.x += gapWidth;
        icon = Instantiate(Boss, map);
        icon.transform.position = moveVec;
    }



    /**********************************************************
    * 불러온 맵으로 생성
    ***********************************************************/





    // test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - 1번 누름 맵 생성");
            MakeMap();
            //MakeMap(lineNum);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"{GetType()} - 2번 누름 저장해보기");



            GameManager.instance.SaveGame();
        }


        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Debug.Log($"{GetType()} - 2번 누름 저장해보기");

        //stageDate.stageNum = 1;
        //stageDates.Add(Data);

        //stageDate.stageNum = 2;
        //stageDates.Add(Data);





        //    DataManager.instance.mainMapData = mainMapData;

        //    GameManager.instance.SaveGame();
        //}
    }

}
