/******************************************************************************
* MainMapScene의 UI 컨트롤
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapUIController : MonoBehaviour
{
    /******************************************************************************
    * UI 상태관리
    *******************************************************************************/
    public enum UiState
    {
        Nothing,   // 아무것도 뜨지 않은 상태
        ShowPopUp, // 팝업창이 떠있는 상태
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
    * 몬스터 아이콘을 눌렀을 때
    ***********************************************************/
    public void ClickBtnMonster()
    {
        // 
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 몬스터 버튼 누름");
        }
    }
}
