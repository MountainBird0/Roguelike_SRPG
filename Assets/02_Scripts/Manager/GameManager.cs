/******************************************************************************
* �������� ���� ������ ����
*******************************************************************************/
using UnityEngine;

[DefaultExecutionOrder((int)SEO.GameManager)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
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

    [HideInInspector]
    public bool hasSaveData = false;

    private void Start()
    {
        DataManager.instance.LoadDefaultData();
        hasSaveData = DataManager.instance.LoadPlayingData();
    }

    /******************************************************************************
    *  �̾��ϱ� ���� - 
    *******************************************************************************/
    public void StarContinueGame()
    {
        Debug.Log($"{GetType()} - �̾��ϱ�");

        // �� �̵�
        GlobalSceneManager.instance.GoLodingScene();

        // �� �Ŵ��� �� ����
    }

    /******************************************************************************
    * ���ο� ���� ���� - 
    *******************************************************************************/
    public void StartNewGame()
    {
        Debug.Log($"{GetType()} - �� ���� ����");
  
        hasSaveData = false;
        DataManager.instance.DeleteSaveData();
   
        // �� �̵�
        GlobalSceneManager.instance.GoLodingScene();
    }

    /******************************************************************************
    * ���� ���� - ����Scene
    *******************************************************************************/
    public void SaveGame()
    {
        Debug.Log($"{GetType()} - ����");
        hasSaveData = true;

        DataManager.instance.SaveDate();
    }

}
