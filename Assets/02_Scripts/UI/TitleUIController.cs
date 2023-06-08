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

    public TitleUIManager UIMgr;

    public GameObject NewGamePopUp;
    public GameObject ContinuePopUp;
    // 유물, 업적 등 추가


    /**********************************************************
    * 이어하기 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickBtnContinue()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            ContinuePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * 이어하기 버튼을 누른 후 O 버튼
    ***********************************************************/
    public void ClickBtnContinueYes()
    {
        
    }
    /**********************************************************
    * 이어하기 버튼을 누른 후 X 버튼
    ***********************************************************/
    public void ClickBtnContinueNo()
    {
        ContinuePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }


    /**********************************************************
    * 새로하기 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickBtnNewGame()
    {
        if(currentState.Equals(UiState.Nothing))
        {
            NewGamePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 O 버튼
    ***********************************************************/
    public void ClickBtnNewGameYes()
    {
        GameManager.instance.StartNewGame();
    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 X 버튼
    ***********************************************************/
    public void ClickBtnNewGameNo()
    {
        NewGamePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }


    /**********************************************************
    * 유물 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickBtnArtifact()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 유물 누름");
        }
    }


    /**********************************************************
    * 업적 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickBtnAchievement()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 업적 누름");
        }
    }
}
