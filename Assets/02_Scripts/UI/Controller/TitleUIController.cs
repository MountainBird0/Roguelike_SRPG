/******************************************************************************
* TitleScene의 UI 컨트롤
*******************************************************************************/
using UnityEngine;

public class TitleUIController : MonoBehaviour
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

    #region 팝업
    [SerializeField]
    private GameObject NewGamePopUp;
    [SerializeField]
    private GameObject ContinuePopUp;
    [SerializeField]
    private GameObject DownloadPopUp;
    [SerializeField]
    private GameObject CheckPopUp;
    #endregion
    // 유물, 업적 등 추가

    private void Awake()
    {
        currentState = UiState.ShowPopUp;
    }

    /**********************************************************
    * 리소스 다운로드 팝업 활성화 / 비활성화
    ***********************************************************/
    public void ShowDownloadPopUp()
    {
        DownloadPopUp.SetActive(true);
        currentState = UiState.ShowPopUp;
    }
    /**********************************************************
    * 리소스 다운로드 팝업 비활성화
    ***********************************************************/
    public void HideDownloadPopUp()
    {
        DownloadPopUp.SetActive(false);
        currentState = UiState.Nothing;
    }

    /**********************************************************
    * 리소스 확인 팝업 비활성화
    ***********************************************************/
    public void HideCheckPopUp()
    {        
        CheckPopUp.SetActive(false);
        currentState = UiState.Nothing;
    }



    /**********************************************************
    * 리소스 다운로드 확인버튼
    ***********************************************************/
    public void ClickBtnDownloadYes()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            Debug.LogWarning($"{GetType()} - 다운로드 누름");
            DownloadManager.instance.Download();
            currentState = UiState.Nothing;
        }
    }
    /**********************************************************
    * 리소스 다운로드 종료버튼
    ***********************************************************/
    public void ClickBtnDownloadNo()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {

        }
    }




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
        if (currentState.Equals(UiState.ShowPopUp))
        {
            GameManager.instance.StarContinueGame();
        }
    }
    /**********************************************************
    * 이어하기 버튼을 누른 후 X 버튼
    ***********************************************************/
    public void ClickBtnContinueNo()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            ContinuePopUp.SetActive(false);
            currentState = UiState.Nothing;
        }
    }

    /**********************************************************
    * 새로하기 버튼을 눌렀을 때
    ***********************************************************/
    public void ClickBtnNewGame()
    {
        if(currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - 새로하기 누름");
            NewGamePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 O 버튼
    ***********************************************************/
    public void ClickBtnNewGameYes()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            GameManager.instance.StartNewGame();
        }
    }
    /**********************************************************
    * 새로하기 버튼을 누른 후 X 버튼
    ***********************************************************/
    public void ClickBtnNewGameNo()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            NewGamePopUp.SetActive(false);
            currentState = UiState.Nothing; 
        }
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
