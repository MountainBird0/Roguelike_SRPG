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
    * 새로운 게임 시작을 위해 - 새 게임 누르고 o 버튼을 누르면 실행
    *******************************************************************************/
    public void InputNewGame()
    {
        GameManager.instance.StartNewGame();
    }
}
