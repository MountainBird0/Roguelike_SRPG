/******************************************************************************
* TitleScene�� UI ��Ʈ��
*******************************************************************************/
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    /******************************************************************************
    * UI ���°���
    *******************************************************************************/
    private enum UiState
    {
        Nothing,   // �ƹ��͵� ���� ���� ����
        ShowPopUp, // �˾�â�� ���ִ� ����
    }

    private UiState currentState;

    public TitleUIManager UIMgr;

    public GameObject NewGamePopUp;
    public GameObject ContinuePopUp;
    // ����, ���� �� �߰�

    /**********************************************************
    * �̾��ϱ� ��ư�� ������ ��
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
    * �̾��ϱ� ��ư�� ���� �� O ��ư
    ***********************************************************/
    public void ClickBtnContinueYes()
    {
        GameManager.instance.StarContinueGame();
    }
    /**********************************************************
    * �̾��ϱ� ��ư�� ���� �� X ��ư
    ***********************************************************/
    public void ClickBtnContinueNo()
    {
        ContinuePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }

    /**********************************************************
    * �����ϱ� ��ư�� ������ ��
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
    * �����ϱ� ��ư�� ���� �� O ��ư
    ***********************************************************/
    public void ClickBtnNewGameYes()
    {
        GameManager.instance.StartNewGame();
    }
    /**********************************************************
    * �����ϱ� ��ư�� ���� �� X ��ư
    ***********************************************************/
    public void ClickBtnNewGameNo()
    {
        NewGamePopUp.SetActive(false);
        currentState = UiState.Nothing;
    }

    /**********************************************************
    * ���� ��ư�� ������ ��
    ***********************************************************/
    public void ClickBtnArtifact()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - ���� ����");
        }
    }

    /**********************************************************
    * ���� ��ư�� ������ ��
    ***********************************************************/
    public void ClickBtnAchievement()
    {
        if (currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - ���� ����");
        }
    }
}
