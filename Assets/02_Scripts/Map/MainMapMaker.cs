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

    private void Awake()
    {
        StageLevels = new Dictionary<string, StageLevelData>();
    }

    public void Start()
    {

    }

    /**********************************************************
    * 맵 생성
    ***********************************************************/
    private void MakeMap()
    {
        // 세이브 데이터 있으면
        // 세이브 데이터 없으면
    }

    /**********************************************************
    * 새로운 맵 생성
    ***********************************************************/
    public void MakeNewMap()
    {
        var stageNum = GameManager.instance.currentStage;
        
        StageLevels = DataManager.instance.StageLevels;
        StageLevelData sld;

        if (StageLevels.TryGetValue(stageNum.ToString(), out sld))
        {
            for(int i = 0; i < sld.lineNum; i++)
            {

            }

        }


        switch (stageNum)
        {
            case 1:
                Debug.Log($"{GetType()} - stage1 만듬");
                //for(int i = 0; i < lineNum.stage1Line; i++)
                //{
                //    if(i == 0)
                //    {
                //        // 첫줄
                //    }
                //    else if(i == lineNum.stage1Line - 1)
                //    {
                //        // 마지막 줄
                //    }



                //}


                break;

            case 2:
                Debug.Log($"{GetType()} - stage2 만듬");
                break;

            case 3:
                Debug.Log($"{GetType()} - stage3 만듬");
                break;

            default:
                Debug.Log($"{GetType()} - 무한맵 만듬");
                break;
        }





        

    }

    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    ***********************************************************/
    public void MakeNewData(int lineNum)
    {
        for(int i = 0; i < lineNum; i++)
        {

        }
        



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
            MakeNewMap();
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
