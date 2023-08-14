/******************************************************************************
* MainMapScene�� UI ��Ʈ��
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


    public void InitializeButtons()
    {
        foreach (var button in unitButtons)
        {
            button.Value.onClick.AddListener(() => ClickBtnUnit(button.Key));
        }
    }


    /******************************************************************************
    * Unit ������ Ŭ��
    *******************************************************************************/
    private void ClickBtnUnit(string clickedButtonName)
    {
        unitName = clickedButtonName;
        bigImage.GetComponent<Image>().sprite = pool.bigImages[unitName];
    }




    /******************************************************************************
    * Unit ������ Ŭ��
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
    * UnitWindow ���� Ŭ�� // �ٸ� window�� �����ؼ� ��� �������� Ȯ��
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
    * ���̵�� ��ų��ư Ŭ��
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
    * �ڷΰ��� ��ư Ŭ��
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
