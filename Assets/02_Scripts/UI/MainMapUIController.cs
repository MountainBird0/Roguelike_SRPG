/******************************************************************************
* MainMapScene�� UI ��Ʈ��
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapUIController : MonoBehaviour
{
    [HideInInspector]
    public string unitName;

    public Canvas unitCanvas;

    public Image bigImage;


    public ImagePool pool;

    public Dictionary<string, Button> unitButtons = new();

    /******************************************************************************
    * UI ���°���
    *******************************************************************************/
    private enum UiState
    {
        Nothing,        // �ƹ��͵� ���� ���� ����
        ShowUnitWindow, // �˾�â�� ���ִ� ����
        ShowSetting
    }
    private enum UnitWindowState
    {
        Nothing,
        ShowSkill,
        ShowEquipment,
        ShowStats
    }

    private UiState currentUiState = UiState.Nothing;
    private UnitWindowState currentUnitState = UnitWindowState.Nothing;

    public void InitializeButtons()
    {
        foreach (var button in unitButtons)
        {
            button.Value.onClick.AddListener(() => ClickBtnUnit(button.Key));
        }
    }

    private void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;
        bigImage.sprite = pool.bigImages[unitName];
    }






    public void ClickBtnUnitIcon()
    {
        if(currentUiState.Equals(UiState.Nothing))
        {
            unitCanvas.gameObject.SetActive(true);
            currentUiState = UiState.ShowUnitWindow;
        }
    }

    public void ClickBtnUnitExit()
    {
        if (!currentUiState.Equals(UiState.Nothing))
        {
            unitCanvas.gameObject.SetActive(false);
            currentUiState = UiState.ShowUnitWindow;
        }
    }





    // �ӽ�
    /**********************************************************
    * ���� �� ���θ޴���
    ***********************************************************/
    public void ClickBtnEXID()
    {
        GameManager.instance.SaveGame();
        GlobalSceneManager.instance.GoTitleScene();
    }


}
