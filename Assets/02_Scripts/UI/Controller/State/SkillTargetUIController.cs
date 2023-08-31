using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetUIController : MonoBehaviour
{
    public Canvas skillTargetCanvas;

    /**********************************************************
    * 취소 버튼
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - 취소 누름");
        StateMachineController.instance.ChangeTo<SkillSelectionState>();
    }


    /**********************************************************
    * 공격확인 버튼
    ***********************************************************/
    public void ClickBtnAttack()
    {
        Debug.Log($"{GetType()} - 공격 누름");
        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


    /**********************************************************
    * 선택 캔버스 활성화/비활성화
    ***********************************************************/
    public void EnableCanvas()
    {
        skillTargetCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas()
    {
        skillTargetCanvas.gameObject.SetActive(false);
    }
}
