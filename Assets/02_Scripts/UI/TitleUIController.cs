/******************************************************************************
* TitleScene�� UI ��Ʈ��
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    /******************************************************************************
    * UI ���°���
    *******************************************************************************/
    public enum UiState
    {
        Nothing,   // �ƹ��͵� ���� ���� ����
        ShowPopUp, // �˾�â�� ���ִ� ����
    }

    private UiState currentState;

    public UiState CurrentState
    {
        get { return currentState; }
        set
        {
            currentState = value;
            switch (currentState)
            {
                case UiState.Nothing:
                    break;
                case UiState.ShowPopUp:
                    break;
            }
        }
    }


    public GameObject CheckNewGamePopUp;
    // �̾��ϱ�, ����, ���� �� �߰�

    /**********************************************************
    * �̾��ϱ� ��ư�� ������ ��
    ***********************************************************/
    public void ClickButtonContinue()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - �̾��ϱ� ����");
        }
    }

    /**********************************************************
    * �����ϱ� ��ư�� ������ ��
    ***********************************************************/
    public void ClickButtonNewGame()
    {
        if(currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - �����ϱ� ����");

            CheckNewGamePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * �����ϱ� ��ư�� ���� �� O ��ư
    ***********************************************************/
    public void ClickButtonNewGameYes()
    {
        Debug.Log($"{GetType()} - �����ϱ� ���� �� O ��ư");

        TitleUIManager.instance.NewGameStart();

    }
    /**********************************************************
    * �����ϱ� ��ư�� ���� �� X ��ư
    ***********************************************************/
    public void ClickButtonNewGameNo()
    {
        Debug.Log($"{GetType()} - �����ϱ� ���� �� X ��ư");

        CheckNewGamePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }



    /**********************************************************
    * ���� ��ư�� ������ ��
    ***********************************************************/
    public void ClickButtonArtifact()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - ���� ����");
        }
    }

    /**********************************************************
    * ���� ��ư�� ������ ��
    ***********************************************************/
    public void ClickButtonAchievement()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - ���� ����");
        }
    }
}
