using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionUIController : MonoBehaviour
{
    public Canvas arrowSelectionCanvas;

    /**********************************************************
    * 스킬 취소 버튼
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - 이거누름");
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    /**********************************************************
    * 선택 캔버스 활성화/비활성화
    ***********************************************************/
    public void EnableCanvas()
    {
        arrowSelectionCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas()
    {
        arrowSelectionCanvas.gameObject.SetActive(false);
    }
}
