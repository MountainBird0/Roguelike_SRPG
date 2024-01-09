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

    #region �˾�
    [SerializeField]
    private GameObject NewGamePopUp;
    [SerializeField]
    private GameObject ContinuePopUp;
    [SerializeField]
    private GameObject DownloadPopUp;
    [SerializeField]
    private GameObject CheckPopUp;
    #endregion
    // ����, ���� �� �߰�

    private void Awake()
    {
        currentState = UiState.ShowPopUp;
    }

    /**********************************************************
    * ���ҽ� �ٿ�ε� �˾� Ȱ��ȭ / ��Ȱ��ȭ
    ***********************************************************/
    public void ShowDownloadPopUp()
    {
        DownloadPopUp.SetActive(true);
        currentState = UiState.ShowPopUp;
    }
    /**********************************************************
    * ���ҽ� �ٿ�ε� �˾� ��Ȱ��ȭ
    ***********************************************************/
    public void HideDownloadPopUp()
    {
        DownloadPopUp.SetActive(false);
        currentState = UiState.Nothing;
    }

    /**********************************************************
    * ���ҽ� Ȯ�� �˾� ��Ȱ��ȭ
    ***********************************************************/
    public void HideCheckPopUp()
    {        
        CheckPopUp.SetActive(false);
        currentState = UiState.Nothing;
    }



    /**********************************************************
    * ���ҽ� �ٿ�ε� Ȯ�ι�ư
    ***********************************************************/
    public void ClickBtnDownloadYes()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            Debug.LogWarning($"{GetType()} - �ٿ�ε� ����");
            DownloadManager.instance.Download();
            currentState = UiState.Nothing;
        }
    }
    /**********************************************************
    * ���ҽ� �ٿ�ε� �����ư
    ***********************************************************/
    public void ClickBtnDownloadNo()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {

        }
    }




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
        if (currentState.Equals(UiState.ShowPopUp))
        {
            GameManager.instance.StarContinueGame();
        }
    }
    /**********************************************************
    * �̾��ϱ� ��ư�� ���� �� X ��ư
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
    * �����ϱ� ��ư�� ������ ��
    ***********************************************************/
    public void ClickBtnNewGame()
    {
        if(currentState.Equals(UiState.Nothing))
        {
            Debug.Log($"{GetType()} - �����ϱ� ����");
            NewGamePopUp.SetActive(true);
            currentState = UiState.ShowPopUp;
        }
    }
    /**********************************************************
    * �����ϱ� ��ư�� ���� �� O ��ư
    ***********************************************************/
    public void ClickBtnNewGameYes()
    {
        if (currentState.Equals(UiState.ShowPopUp))
        {
            GameManager.instance.StartNewGame();
        }
    }
    /**********************************************************
    * �����ϱ� ��ư�� ���� �� X ��ư
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
