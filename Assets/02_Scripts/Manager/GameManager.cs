/******************************************************************************
* ������ ���������� ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    }

    private void Update()
    {

        
    }
}
