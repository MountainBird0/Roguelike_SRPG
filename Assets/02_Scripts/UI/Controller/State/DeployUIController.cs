/**********************************************************
* DeployState의 UI 컨트롤
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
    * 버튼 누르면 누른 버튼의 이름으로 unitName변경
    ***********************************************************/
    private void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;
        EnableGuide();
    }

    /**********************************************************
    * 배치 가능한 유닛 활성화/비활성화
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
    * 유닛 배치 가이드 활성화/비활성화
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
    * 배치 버튼 활성화/비활성화
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
    * 배치 완료
    ***********************************************************/
    public void ClickBtnFinish()
    {
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }
}
