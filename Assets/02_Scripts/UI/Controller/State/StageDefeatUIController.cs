using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDefeatUIController : MonoBehaviour
{
    public Canvas defeatCanvas;

    public void EnableCanvas()
    {
        GameManager.instance.hasSaveData = false;
        defeatCanvas.gameObject.SetActive(true);
    }

    /**********************************************************
    * 타이틀 이동 버튼
    ***********************************************************/
    public void ClickBtnEnd()
    {
        GlobalSceneManager.instance.GoTitleScene();
    }
}
