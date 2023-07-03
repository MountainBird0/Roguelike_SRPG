/******************************************************************************
* �������� Scene�̵� ����
*******************************************************************************/
using UnityEngine.SceneManagement;
using UnityEngine;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager instance;

    private AsyncOperation operation;

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


    /******************************************************************************
    * �񵿱� Scene �̵�
    *******************************************************************************/
    public AsyncOperation GoScene(int sceneNum)
    {
        operation = SceneManager.LoadSceneAsync(sceneNum);
        return operation;
    }


    /******************************************************************************
    * �ε������� �̵�
    *******************************************************************************/
    public void GoLodingScene()
    {
        SceneManager.LoadScene(5);
    }


    /******************************************************************************
    * Ÿ��Ʋ������ �̵�
    *******************************************************************************/
    public void GoTitleScene()
    {
        SceneManager.LoadScene(0);
    }


    /******************************************************************************
    * ���ξ����� �̵�
    *******************************************************************************/
    public AsyncOperation GoMainScene()
    {
        operation = SceneManager.LoadSceneAsync(1);
        return operation;
    }


    /******************************************************************************
    * ��Ʋ������ �̵�
    *******************************************************************************/
    public void GoBattleScene()
    {
        SceneManager.LoadScene(2);
    }

}
