/******************************************************************************
* BattleScene의 UI 컨트롤
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapUIController : MonoBehaviour
{
    /**********************************************************
    * 테스트용 클리어 버튼 // 나중에 뒤로가기로
    ***********************************************************/
    public void ClickBtnClear()
    {
        GlobalSceneManager.instance.GoMainScene();
    }
}
