/******************************************************************************
* TitleScene�� UI ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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
    * ���ο� ���� ������ ���� - �� ���� ������ o ��ư�� ������ ����
    *******************************************************************************/
    public void InputNewGame()
    {
        Debug.Log($"{GetType()} - �����ϱ� ���� �� O ��ư");

        GameManager.instance.StartNewGame();
    }
}