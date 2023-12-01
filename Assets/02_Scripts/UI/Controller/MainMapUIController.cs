/******************************************************************************
* MainMapScene�� UI ��Ʈ��
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
    * Unit Ŭ��
    *******************************************************************************/
    private async void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;

        bigImage.GetComponent<Image>().sprite = await AddressableManager.instance.GetImage(unitName);
    }

    /******************************************************************************
    * Skill Ŭ��
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
    * Unit ������ Ŭ��
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
    * UnitWindow ���� Ŭ�� // �ٸ� window�� �����ؼ� ��� �������� Ȯ��
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
    * ���̵�� ��ų��ư Ŭ��
    *******************************************************************************/
    public void ClickBtnSideBarSkill()
    {
        if(currentUiState.Equals(UiState.ShowUnitWindow))
        {
            currentUnitWindowState = UnitWindowState.ShowSkill;

            skillSlots = manager.skillSlots; // �̰� ���߿� private�� ����ϰ�
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
    * �ڷΰ��� ��ư Ŭ��
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
    * skill check Ȱ��ȭ
    *******************************************************************************/
    public void ChangeToTouchable(int id)
    {
        skillSlots[id].GetComponent<SkillSlot>().check.SetActive(false);
    }

    /**********************************************************
    * currentEquipSkills�� ����
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
