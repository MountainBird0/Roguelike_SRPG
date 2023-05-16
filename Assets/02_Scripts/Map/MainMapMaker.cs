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




    private StageData stageData;

    private void Awake()
    {
        stageData = new StageData()
        {
            iconCounts = new Queue<int>(),
            iconTypes = new Queue<ICON>()
        };
    }

    public void Start()
    {
        MakeMap();
    }

    /**********************************************************
    * 맵 생성
    ***********************************************************/
    private void MakeMap()
    {
        stageData = DataManager.instance.stageData;

        GameObject icon;
        ICON iconType;

        Debug.Log($"{GetType()} - 맵생성 들어감");
        Debug.Log($"{GetType()} - 라인 수 {stageData.lineCount}");

        for (int i = 0; i < stageData.lineCount; i++)
        {
            Debug.Log($"{GetType()} - 이거 몇번?");

            iconType = stageData.iconTypes.Dequeue();
            
            switch(iconType)
            {
                case ICON.MONSTER:
                    icon = Instantiate(Monster, map);
                    break;


            }
            

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
                
            }
            // 마지막 라인
            else if (i == lineNum - 1)
            {
                
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



    // test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - 1번 누름 맵 생성");
            
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
