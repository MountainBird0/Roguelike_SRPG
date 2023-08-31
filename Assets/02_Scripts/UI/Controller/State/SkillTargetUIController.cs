using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetUIController : MonoBehaviour
{
    public Canvas skillTargetCanvas;

    /**********************************************************
    * ��� ��ư
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - ��� ����");
        StateMachineController.instance.ChangeTo<SkillSelectionState>();
    }


    /**********************************************************
    * ����Ȯ�� ��ư
    ***********************************************************/
    public void ClickBtnAttack()
    {
        Debug.Log($"{GetType()} - ���� ����");
        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
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
