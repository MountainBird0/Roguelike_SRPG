/******************************************************************************
* 게임을 전반적으로 관리
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
    * 새로운 게임 시작 - 
    *******************************************************************************/
    public void StartNewGame()
    {
        Debug.Log($"{GetType()} - 새 게임 시작");

        // 씬 이동
        GlobalSceneManager.instance.GoLodingScene();

        // 맵 매니저 맵 생성
    }

}
