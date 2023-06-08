/******************************************************************************
* 전반적인 Scene이동 관리
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager instance;

    private AsyncOperation operation;


    public void Awake()
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
        SceneManager.LoadScene(0);
    }

    /******************************************************************************
    * 타이틀씬으로 이동
    *******************************************************************************/
    public void GoTitleScene()
    {
        SceneManager.LoadScene(1);
    }

    /******************************************************************************
    * 메인씬으로 이동
    *******************************************************************************/
    public AsyncOperation GoMainScene()
    {
        operation = SceneManager.LoadSceneAsync(2);
        return operation;
    }

}
