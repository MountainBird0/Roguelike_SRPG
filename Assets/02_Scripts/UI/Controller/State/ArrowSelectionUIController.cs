using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionUIController : MonoBehaviour
{
    public Canvas arrowSelectionCanvas;

    public GameObject ChooseList;

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
        ChooseList.transform.DORotate(new Vector3(0, 0, 150), 0.2f).From(true);
    }
    public void DisableCanvas()
    {
        arrowSelectionCanvas.gameObject.SetActive(false);
    }
}
