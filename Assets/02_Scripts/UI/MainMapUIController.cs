/******************************************************************************
* MainMapScene의 UI 컨트롤
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMapUIController : MonoBehaviour
{
    [HideInInspector]
    public string unitName;

    public GameObject bigImage;
    //public Image bigImage;
    public MainMapUIManager manager;

    public ImagePool pool;

    [Header("OB_UI")]
    public GameObject unitCanvas;

    public GameObject ScrollView_Units;
    public GameObject Image_Unit;
    public GameObject Panel_StatWindow;
    public GameObject Panel_SkillWindow;
    public List<Image> skillSlots;

    [Header("SideBar")]
    public GameObject Button_Skill;
    public GameObject Button_Equipment;
    public GameObject Button_Stat;

    [Header("Pos")]
    public Transform imagePos_L;
    public Transform imagePos_R;


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
    private UnitWindowState currentUnitWindowState = UnitWindowState.Nothing;

    private void Start()
    {
        manager.CreateUnitSlot(this);
    }


    public void InitializeButtons()
    {
        foreach (var button in unitButtons)
        {
            button.Value.onClick.AddListener(() => ClickBtnUnit(button.Key));
        }
    }


    /******************************************************************************
    * Unit 아이콘 클릭
    *******************************************************************************/
    private void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;
        bigImage.GetComponent<Image>().sprite = pool.bigImages[unitName];
    }




    /******************************************************************************
    * Unit 아이콘 클릭
    *******************************************************************************/
    public void ClickBtnUnitIcon()
    {
        if(currentUiState.Equals(UiState.Nothing))
        {
            unitCanvas.SetActive(true);
            currentUiState = UiState.ShowUnitWindow;
        }
    }


    /******************************************************************************
    * UnitWindow 끄기 클릭 // 다른 window랑 통합해서 사용 가능한지 확인
    *******************************************************************************/
    public void ClickBtnUnitExit()
    {
        if (!currentUiState.Equals(UiState.Nothing))
        {
            unitCanvas.SetActive(false);
            currentUiState = UiState.Nothing;
        }
    }


    /******************************************************************************
    * 사이드바 스킬버튼 클릭
    *******************************************************************************/
    public void ClickBtnSideBarSkill()
    {
        if(currentUiState.Equals(UiState.ShowUnitWindow))
        {
            currentUnitWindowState = UnitWindowState.ShowSkill;

            manager.CreateSkillSlot(unitName);
            manager.SetStatWindow(unitName);

            Panel_SkillWindow.SetActive(true);
            Panel_StatWindow.SetActive(true);
            ScrollView_Units.SetActive(false);
            bigImage.transform.DOMove(imagePos_L.position, 0.5f);
        }
    }

    /******************************************************************************
    * 뒤로가기 버튼 클릭
    *******************************************************************************/
    public void ClickBtnUnitBack()
    {
        if (!currentUnitWindowState.Equals(UnitWindowState.Nothing))
        {
            currentUnitWindowState = UnitWindowState.Nothing;

            manager.ClearSkillSlot();

            Panel_SkillWindow.SetActive(false);
            Panel_StatWindow.SetActive(false);
            ScrollView_Units.SetActive(true);
            bigImage.transform.DOMove(imagePos_R.position, 0.5f);
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
