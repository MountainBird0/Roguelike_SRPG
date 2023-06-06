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

    public bool isNewGame;

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

        isNewGame = false;
    }

    /******************************************************************************
    * ���ο� ���� ���� - 
    *******************************************************************************/
    public void StartNewGame()
    {
        Debug.Log($"{GetType()} - �� ���� ����");

        currentStage = 1;
        isNewGame = true;

        // �� �̵�
        GlobalSceneManager.instance.GoLodingScene();
 

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

        //DataManager.instance.SaveDate(); // ������ ����
        DataManager.instance.SaveTemp(); // ������ ����
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            int a = Random.Range(1, 3);
            Debug.Log(a);
        }
        
    }
}
