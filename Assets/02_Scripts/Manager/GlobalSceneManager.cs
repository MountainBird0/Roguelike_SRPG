/******************************************************************************
* 전반적인 Scene이동 관리
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
    * 비동기 Scene 이동
    *******************************************************************************/
    public AsyncOperation GoScene(int sceneNum)
    {
        operation = SceneManager.LoadSceneAsync(sceneNum);
        return operation;
    }


    /******************************************************************************
    * 로딩씬으로 이동
    *******************************************************************************/
    public void GoLodingScene()
    {
        SceneManager.LoadScene(5);
    }


    /******************************************************************************
    * 타이틀씬으로 이동
    *******************************************************************************/
    public void GoTitleScene()
    {
        SceneManager.LoadScene(0);
    }


    /******************************************************************************
    * 메인씬으로 이동
    *******************************************************************************/
    public AsyncOperation GoMainScene()
    {
        operation = SceneManager.LoadSceneAsync(1);
        return operation;
    }


    /******************************************************************************
    * 배틀씬으로 이동
    *******************************************************************************/
    public void GoBattleScene()
    {
        SceneManager.LoadScene(2);
    }

}
