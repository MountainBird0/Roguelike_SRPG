/******************************************************************************
* 게임을 전반적으로 관리
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentStage;




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


        currentStage = 1;
        // 씬 이동
        GlobalSceneManager.instance.GoLodingScene();

        DataManager.instance.LoadNewData();

        // 맵 매니저 맵 생성
    }

    /******************************************************************************
    *  이어하기 시작 - 
    *******************************************************************************/
    public void StarContinueGame()
    {
        Debug.Log($"{GetType()} - 이어하기");

        // 씬 이동
        GlobalSceneManager.instance.GoLodingScene();

        // 맵 매니저 맵 생성
    }











    /******************************************************************************
    * 게임 저장 - 메인Scene
    *******************************************************************************/
    public void SaveGame()
    {
        Debug.Log($"{GetType()} - 저장");

        DataManager.instance.SaveDate(); // 데이터 저장
        //DataManager.instance.SaveTemp(); // 데이터 저장
    }

}
