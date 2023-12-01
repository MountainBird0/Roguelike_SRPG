/******************************************************************************
* MainMapScene의 UI 컨트롤
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMapUIController : MonoBehaviour
{
    public GameObject bigImage;
    //public Image bigImage;
    public MainMapUIManager manager;
    public GameObject mainMapInput;

    public ImagePool pool;

    [Header("OB_UI")]
    public GameObject unitCanvas;

    public GameObject ScrollView_Units;
    public GameObject Image_Unit;
    public GameObject Panel_StatWindow;

    [Header("Skill")]
    public GameObject Panel_SkillWindow;
    public GameObject Panel_SkillInfo;
    public List<GameObject> equipSkillSlots;

    [Header("SideBar")]
    public GameObject Button_Skill;
    public GameObject Button_Equipment;
    public GameObject Button_Stat;

    [Header("Pos")]
    public Transform imagePos_L;
    public Transform imagePos_R;


    private string unitName;

    public Dictionary<string, Button> unitButtons = new();
    public Dictionary<int, Button> skillButtons = new();

    private Dictionary<int, GameObject> skillSlots = new();

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


    public void InitializeUnitButtons()
    {
        foreach (var button in unitButtons)
        {
            button.Value.onClick.AddListener(() => ClickBtnUnit(button.Key));
            if (unitName == null)
            {
                unitName = button.Key;
            }
        }
    }

    /******************************************************************************
    * Unit 클릭
    *******************************************************************************/
    private async void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;

        bigImage.GetComponent<Image>().sprite = await AddressableManager.instance.GetImage(unitName);
    }

    /******************************************************************************
    * Skill 클릭
    *******************************************************************************/
    public void ClickSkill(int skillNum)
    {
        if(!Panel_SkillInfo.activeSelf)
        {
            Panel_SkillInfo.SetActive(true);
        }
        manager.SetSkillInfoWindow(skillNum);
    }

    /******************************************************************************
    * Unit 아이콘 클릭
    *******************************************************************************/
    public void ClickBtnUnitIcon()
    {
        if(currentUiState.Equals(UiState.Nothing))
        {
            mainMapInput.SetActive(false);

            unitCanvas.SetActive(true);

            ClickBtnUnit(unitName);


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
            mainMapInput.SetActive(true);


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

            skillSlots = manager.skillSlots; // 이거 나중에 private로 사용하게
            manager.CreateSkillSlot(unitName, equipSkillSlots);
            manager.SetStatWindow(unitName);
            // manager.SetSkillWindow(0);

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

            Panel_SkillWindow.SetActive(false);
            Panel_SkillInfo.SetActive(false);
            Panel_StatWindow.SetActive(false);
            ScrollView_Units.SetActive(true);
            bigImage.transform.DOMove(imagePos_R.position, 0.5f);

            SetEquipSkills();
        }
    }

    /******************************************************************************
    * skill check 활성화
    *******************************************************************************/
    public void ChangeToTouchable(int id)
    {
        skillSlots[id].GetComponent<SkillSlot>().check.SetActive(false);
    }

    /**********************************************************
    * currentEquipSkills에 저장
    ***********************************************************/
    private void SetEquipSkills()
    {
        IntListData skillList = new();

        for (int i = 0; i < equipSkillSlots.Count; i++)
        {
            int skillId = equipSkillSlots[i].GetComponent<SkillSlot>().id;

            if (skillId != -1)
            {
                skillList.list.Add(skillId);
            }
        }
        DataManager.instance.currentEquipSkills[unitName] = skillList;
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
