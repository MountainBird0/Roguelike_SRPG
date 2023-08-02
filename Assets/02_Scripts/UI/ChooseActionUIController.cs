using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseActionUIController : MonoBehaviour
{
    public Canvas actionCanvas;

    /**********************************************************
    * 턴 종료 버튼
    ***********************************************************/
    public void ClickBtnEnd()
    {
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * 선택 캔버스 활성화/비활성화
    ***********************************************************/
    public void EnableCanvas()
    {
        actionCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas()
    {
        actionCanvas.gameObject.SetActive(false);
    }
}
