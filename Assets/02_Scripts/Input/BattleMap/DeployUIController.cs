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
    * ��ư ������ ���� ��ư�� �̸����� unitName����
    ***********************************************************/
    private void ClickBtnUnit(string clickedButtonName)
    {
        EnableGuide();
        unitName = clickedButtonName;
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
