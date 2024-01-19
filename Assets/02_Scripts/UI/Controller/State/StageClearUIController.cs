using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearUIController : MonoBehaviour
{
    public Canvas clearCanvas;

    public GameObject EndButton;

    public void EnableCanvas()
    {
        clearCanvas.gameObject.SetActive(true);
        SetClearWindow();
    }

    /**********************************************************
    * 클리어윈도우 세팅
    ***********************************************************/
    private void SetClearWindow()
    {
        BattleMapUIManager.instance.CreateResultSlot();
    }

    /**********************************************************
    * 메인화면 이동 버튼
    ***********************************************************/
    public void ClickBtnEnd()
    {
        GlobalSceneManager.instance.GoMainScene();
    }

    /**********************************************************
    * 배치완료 버튼 활성화 / 비활성화
    ***********************************************************/
        public void EnableEndButton()
        {
            EndButton.SetActive(true);
        }
}
