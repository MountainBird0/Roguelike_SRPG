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



    private Dictionary<string, StageLevelData> StageLevels;

    private void Awake()
    {
        StageLevels = new Dictionary<string, StageLevelData>();
    }

    public void Start()
    {

    }

    /**********************************************************
    * �� ����
    ***********************************************************/
    private void MakeMap()
    {
        // ���̺� ������ ������
        // ���̺� ������ ������
    }

    /**********************************************************
    * ���ο� �� ����
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
                Debug.Log($"{GetType()} - stage1 ����");
                //for(int i = 0; i < lineNum.stage1Line; i++)
                //{
                //    if(i == 0)
                //    {
                //        // ù��
                //    }
                //    else if(i == lineNum.stage1Line - 1)
                //    {
                //        // ������ ��
                //    }



                //}


                break;

            case 2:
                Debug.Log($"{GetType()} - stage2 ����");
                break;

            case 3:
                Debug.Log($"{GetType()} - stage3 ����");
                break;

            default:
                Debug.Log($"{GetType()} - ���Ѹ� ����");
                break;
        }





        

    }

    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    ***********************************************************/
    public void MakeNewData(int lineNum)
    {
        for(int i = 0; i < lineNum; i++)
        {

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
            MakeNewMap();
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
