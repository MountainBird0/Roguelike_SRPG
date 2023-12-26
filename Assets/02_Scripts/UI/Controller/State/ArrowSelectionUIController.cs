using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionUIController : MonoBehaviour
{
    public Canvas arrowSelectionCanvas;

    public GameObject ChooseList;

    /**********************************************************
    * ��ų ��� ��ư
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - �̰Ŵ���");
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
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
