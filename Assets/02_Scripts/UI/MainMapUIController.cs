/******************************************************************************
* MainMapScene�� UI ��Ʈ��
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapUIController : MonoBehaviour
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





    /**********************************************************
    * ���� �������� ������ ��
    ***********************************************************/
    public void ClickBtnMonster()
    {
        // 
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - ���� ��ư ����");
        }
    }
}
