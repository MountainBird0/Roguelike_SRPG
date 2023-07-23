using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployUIController : MonoBehaviour
{
    [HideInInspector]
    public string unitName;

    public Canvas guideCanvas;

    public Button warriorButton;
    public Button mageButton;
    public Button rogueButton;


    private Dictionary<string, Button> unitButtons;

    private void Start()
    {
        unitButtons = new Dictionary<string, Button>
        {
            { "Warrior", warriorButton },
            { "Mage", mageButton },
            { "Rogue", rogueButton }
        };
        InitializeButtons();
    }

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
        EnableGuide();
        unitName = clickedButtonName;
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
        }
    }
    public void DisableButton(string buttonName)
    {
        if (unitButtons.ContainsKey(buttonName))
        {
            unitButtons[buttonName].interactable = false;
        }
    }
}
