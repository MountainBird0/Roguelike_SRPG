/******************************************************************************
* TitleScene의 UI 컨트롤
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
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


    public GameObject CheckNewGamePopUp;
    // 이어하기, 유물, 업적 등 추가

    /**********************************************************
    * 이어하기 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickButtonContinue()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 이어하기 누름");
        }
    }

    /**********************************************************
    * 새로하기 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickButtonNewGame()
    {
        if(currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 새로하기 누름");

            CheckNewGamePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 O 버튼
    ***********************************************************/
    public void ClickButtonNewGameYes()
    {
        Debug.Log($"{GetType()} - 새로하기 누른 후 O 버튼");

        TitleUIManager.instance.NewGameStart();

    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 X 버튼
    ***********************************************************/
    public void ClickButtonNewGameNo()
    {
        Debug.Log($"{GetType()} - 새로하기 누른 후 X 버튼");

        CheckNewGamePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }



    /**********************************************************
    * 유물 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickButtonArtifact()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 유물 누름");
        }
    }

    /**********************************************************
    * 업적 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickButtonAchievement()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 업적 누름");
        }
    }
}
