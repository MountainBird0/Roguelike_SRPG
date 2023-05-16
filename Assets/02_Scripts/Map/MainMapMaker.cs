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
    * �� ����
    ***********************************************************/
    private void MakeMap()
    {
        stageData = DataManager.instance.stageData;

        GameObject icon;
        ICON iconType;

        Debug.Log($"{GetType()} - �ʻ��� ��");
        Debug.Log($"{GetType()} - ���� �� {stageData.lineCount}");

        for (int i = 0; i < stageData.lineCount; i++)
        {
            Debug.Log($"{GetType()} - �̰� ���?");

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
                
            }
            // ������ ����
            else if (i == lineNum - 1)
            {
                
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



    // test
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - 1�� ���� �� ����");
            
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
