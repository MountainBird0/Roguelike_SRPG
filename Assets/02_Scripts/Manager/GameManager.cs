/******************************************************************************
* �������� ���� ������ ����
*******************************************************************************/
using System.Collections;
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
        //StartCoroutine(DownloadManager.instance.InitAddressable());
        //StartCoroutine(DownloadManager.instance.CheckUpdateFiles());

        //DataManager.instance.LoadDefaultData();
        //hasSaveData = DataManager.instance.LoadPlayingData();
    }

    private void OnEnable()
    {
        StartCoroutine(GameSetting());
    }


    /******************************************************************************
    *  �̾��ϱ� ���� - 
    *******************************************************************************/
    public void StarContinueGame()
    {
        Debug.Log($"{GetType()} - �̾��ϱ�");
        hasSaveData = DataManager.instance.LoadPlayingData();
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

    public IEnumerator GameSetting()
    {
        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine(DownloadManager.instance.InitAddressable());
        StartCoroutine(DownloadManager.instance.CheckUpdateFiles());

        yield return new WaitForSeconds(0.5f);


        DataManager.instance.LoadDefaultData();
            
    }
}
