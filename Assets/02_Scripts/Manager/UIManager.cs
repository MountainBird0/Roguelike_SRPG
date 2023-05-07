/******************************************************************************
* TitleScene의 UI 관리
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

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
    * 저장된 게임 불러오기 - 이어하기 버튼을 누르면 실행
    *******************************************************************************/



    /******************************************************************************
    * 새로운 게임 시작을 위해
    *******************************************************************************/
    public void NewGameStart()
    {
        Debug.Log($"{GetType()} - 새로하기 누른 후 O 버튼");

        GameManager.instance.StartNewGame();
    }
}
