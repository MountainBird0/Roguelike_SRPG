/******************************************************************************
* TitleScene�� UI ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public static TitleUIManager instance;

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
    * ����� ���� �ҷ����� - �̾��ϱ� ��ư�� ������ ����
    *******************************************************************************/



    /******************************************************************************
    * ���ο� ���� ������ ����552
    *******************************************************************************/
    public void NewGameStart()
    {
        Debug.Log($"{GetType()} - �����ϱ� ���� �� O ��ư");

        GameManager.instance.StartNewGame();
    }
}
