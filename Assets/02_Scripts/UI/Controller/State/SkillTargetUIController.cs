using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetUIController : MonoBehaviour
{
    public Canvas skillTargetCanvas;

    public GameObject ChooseList;

    /**********************************************************
    * ��� ��ư
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - ��� ����");
        if (Turn.skill.data.isDirectional)
        {
            StateMachineController.instance.ChangeTo<ArrowSelectionState>();
            return;
        }
        StateMachineController.instance.ChangeTo<SkillSelectedState>();
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
        ChooseList.transform.DORotate(new Vector3(0, 0, 150), 0.2f).From(true);
    }
    public void DisableCanvas()
    {
        skillTargetCanvas.gameObject.SetActive(false);
    }
}
