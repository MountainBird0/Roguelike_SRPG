/******************************************************************************
* ������ ���������� ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentStage;




    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    /******************************************************************************
    * ���ο� ���� ���� - 
    *******************************************************************************/
    public void StartNewGame()
    {
        Debug.Log($"{GetType()} - �� ���� ����");


        currentStage = 1;
        // �� �̵�
        GlobalSceneManager.instance.GoLodingScene();

        DataManager.instance.LoadNewData();

        // �� �Ŵ��� �� ����
    }

    /******************************************************************************
    *  �̾��ϱ� ���� - 
    *******************************************************************************/
    public void StarContinueGame()
    {
        Debug.Log($"{GetType()} - �̾��ϱ�");

        // �� �̵�
        GlobalSceneManager.instance.GoLodingScene();

        // �� �Ŵ��� �� ����
    }











    /******************************************************************************
    * ���� ���� - ����Scene
    *******************************************************************************/
    public void SaveGame()
    {
        Debug.Log($"{GetType()} - ����");

        DataManager.instance.SaveDate(); // ������ ����
        //DataManager.instance.SaveTemp(); // ������ ����
    }

}
