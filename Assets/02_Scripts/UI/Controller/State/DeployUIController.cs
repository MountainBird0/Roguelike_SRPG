/**********************************************************
* DeployState�� UI ��Ʈ��
***********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployUIController : MonoBehaviour
{
    [HideInInspector]
    public string unitName;

    public Canvas deployCanvas;
    public Canvas guideCanvas;

    public Dictionary<string, Button> unitButtons = new();

    private void InitializeButtons()
    {
        foreach (var button in unitButtons)
        {
            button.Value.onClick.AddListener(() => ClickBtnUnit(button.Key));
        }
    }

    /**********************************************************
    * ��ư ������ ���� ��ư�� �̸����� unitName����
    ***********************************************************/
    private void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;
        EnableGuide();
    }

    /**********************************************************
    * ��ġ ������ ���� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableWindow()
    {
        deployCanvas.gameObject.SetActive(true);
        BattleMapUIManager.instance.CreateDeploySlot();
        InitializeButtons();
    }
    public void DisableWindow()
    {
        deployCanvas.gameObject.SetActive(false);
    }

    /**********************************************************
    * ���� ��ġ ���̵� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableGuide()
    {
        guideCanvas.gameObject.SetActive(true);
    }
    public void DisableGuide()
    {
        guideCanvas.gameObject.SetActive(false);
    }

    /**********************************************************
    * ��ġ ��ư Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableButton(string buttonName)
    {
        if (unitButtons.ContainsKey(buttonName))
        {
            unitButtons[buttonName].interactable = true;
            unitName = null;
        }
    }
    public void DisableButton(string buttonName)
    {
        if (unitButtons.ContainsKey(buttonName))
        {
            unitButtons[buttonName].interactable = false;
            unitName = null;
        }
    }

    /**********************************************************
    * ��ġ �Ϸ�
    ***********************************************************/
    public void ClickBtnFinish()
    {
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }
}
