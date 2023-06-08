/******************************************************************************
* �������� Scene�̵� ����
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
        SceneManager.LoadScene(0);
    }

    /******************************************************************************
    * Ÿ��Ʋ������ �̵�
    *******************************************************************************/
    public void GoTitleScene()
    {
        SceneManager.LoadScene(1);
    }

    /******************************************************************************
    * ���ξ����� �̵�
    *******************************************************************************/
    public AsyncOperation GoMainScene()
    {
        operation = SceneManager.LoadSceneAsync(2);
        return operation;
    }

}
