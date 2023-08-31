using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionUIController : MonoBehaviour
{
    public Canvas arrowSelectionCanvas;

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
    }
    public void DisableCanvas()
    {
        arrowSelectionCanvas.gameObject.SetActive(false);
    }
}
