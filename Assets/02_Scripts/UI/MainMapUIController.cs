/******************************************************************************
* MainMapScene의 UI 컨트롤
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMapUIController : MonoBehaviour
{
    [HideInInspector]
    public string unitName;


    public Image bigImage;


    public ImagePool pool;

    [Header("OB_UI")]
    public GameObject unitCanvas;

    public GameObject ScrollView_Units;
    public GameObject Image_Unit;
    public GameObject Panel_StatWindow;
    public GameObject Panel_SkillWindow;
    public List<Image> skillSlots;


    public Dictionary<string, Button> unitButtons = new();

    /******************************************************************************
    * UI 상태관리
    *******************************************************************************/
    private enum UiState
    {
        Nothing,        // 아무것도 뜨지 않은 상태
        ShowUnitWindow, // 팝업창이 떠있는 상태
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
            unitCanvas.SetActive(true);
            currentUiState = UiState.ShowUnitWindow;
        }
    }

    public void ClickBtnUnitExit()
    {
        if (!currentUiState.Equals(UiState.Nothing))
        {
            unitCanvas.SetActive(false);
            currentUiState = UiState.ShowUnitWindow;
        }
    }





    // 임시
    /**********************************************************
    * 저장 후 메인메뉴로
    ***********************************************************/
    public void ClickBtnEXID()
    {
        GameManager.instance.SaveGame();
        GlobalSceneManager.instance.GoTitleScene();
    }


}
