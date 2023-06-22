/******************************************************************************
* MainMapScene의 UI 컨트롤
*******************************************************************************/
using UnityEngine;

public class MainMapUIController : MonoBehaviour
{
    /******************************************************************************
    * UI 상태관리
    *******************************************************************************/
    private enum UiState
    {
        Nothing,   // 아무것도 뜨지 않은 상태
        ShowPopUp, // 팝업창이 떠있는 상태
    }

    private UiState currentState;

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

    // 임시
    /**********************************************************
    * 저장 후 메인메뉴로
    ***********************************************************/
    public void ClickBtnEXID()
    {
        GameManager.instance.SaveGame();
        GlobalSceneManager.instance.GoTitleScene();
    }


}
