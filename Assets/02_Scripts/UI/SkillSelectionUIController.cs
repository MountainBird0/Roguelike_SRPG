using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionUIController : MonoBehaviour
{
    public Canvas skillSelectionCanvas;


    /**********************************************************
    * 스킬 취소 버튼
    ***********************************************************/
    public void ClickBtnCancel()
    {
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    /**********************************************************
    * 선택 캔버스 활성화/비활성화
    ***********************************************************/
    public void EnableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(false);
    }
}
