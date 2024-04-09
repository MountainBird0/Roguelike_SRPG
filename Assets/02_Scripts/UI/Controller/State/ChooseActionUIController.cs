/**********************************************************
* ChooseActionState�� UI ��Ʈ��
***********************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChooseActionUIController : MonoBehaviour
{
    public Canvas actionCanvas;
    public GraphicRaycaster raycaster;
    
    [Header("SkillSlot")]
    public List<GameObject> skillSlots;
    public List<GameObject> infoWindow;
    public List<SkillInfoWindow> skillWindowInfo;
    public List<GameObject> coolTimeImage;
    public List<TextMeshProUGUI> coolTimeText;

    public StatInfo statInfo;

    [Header("Window")]
    public GameObject skillListWindow;
    public GameObject statWindow;
    public Image unitImage;


    private StringKeyImagePool unitBigPool;
    private IntKeyImagePool skillIconPool;

    public bool isHovor = false;

    private void Start()
    {
        // awake�� ������ �ȴ� ���� �Ű澲��
        //unitImagePool = BattleMapUIManager.instance.unitbt
        skillIconPool = BattleMapUIManager.instance.skillIconPool;
        unitBigPool = BattleMapUIManager.instance.unitBigPool;
    }

    /**********************************************************
    * �� ���� ��ư
    ***********************************************************/
    public void ClickBtnEnd()
    {
        StateMachineController.instance.ChangeTo<TurnEndState>();
    }

    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableCanvas()
    {
        if (Turn.isHumanTurn)
        {
            actionCanvas.gameObject.SetActive(true);
            SetSkillIcon();
            SetStatWindow();
        }
    }
    public void DisableCanvas()
    {
        actionCanvas.gameObject.SetActive(false);
    }

    /**********************************************************
    * ���������� ����
    ***********************************************************/
    private void SetStatWindow()
    {
        var unitName = Turn.unit.unitName;
        unitImage.sprite = unitBigPool.images[unitName];
        statInfo.className.text = unitName;

        var statData = Turn.unit.stats;
        statInfo.level.text = statData.Level.ToString();
        statInfo.hp.text = statData.HP.ToString() + " / " + statData.MaxHP.ToString();

        float hpRatio = (float)statData.HP / statData.MaxHP;
        statInfo.redBar.fillAmount = hpRatio;

        //statWindow.transform.DOMoveY(400, 1.0f);
        //statWindow.transform.DOLocalMoveY(150, 0.5f).From(true);
        var rt = statWindow.GetComponent<RectTransform>();
        rt.DOAnchorPosY(0, 0.5f).From(new Vector2(rt.anchoredPosition.x, 150));
    }

    /**********************************************************
    * ��ų������ ����
    ***********************************************************/
    private void SetSkillIcon()
    {
        for(int i = 0; i < skillSlots.Count; i++)
        {
            var slot = skillSlots[i].GetComponent<BattleSkillSlot>();

            if (Turn.unit.skills.Count <= i)
            {
                Debug.Log($"{GetType()} - �����");
                slot.id = -1;
                slot.image.sprite = BattleMapUIManager.instance.defaultSprite;
                continue;
            }

            var skill = Turn.unit.skills[i].GetComponent<Skill>();   
           
            slot.slotNum = i;
            slot.id = skill.id;
            slot.image.sprite = skill.image;

            if (skill.data.currentCoolTime > 0)
            {
                coolTimeImage[i].SetActive(true);
                coolTimeText[i].text = skill.data.currentCoolTime.ToString();
            }
            else
            {
                coolTimeImage[i].SetActive(false);
            }
        }

        // skillListWindow.transform.DOLocalMove(new Vector3(-300, -300, 0), 0.5f).From(true).SetEase(Ease.OutCirc);
        // skillListWindow.transform.DORotate(new Vector3(0, 0, 150), 0.5f).From(true);
        skillListWindow.transform.DORotate(Vector3.zero, 0.5f).From(new Vector3(0, 0, 150));
    }

    /**********************************************************
    * ��ų����â Ȱ��ȭ / ��Ȱ��ȭ
    ***********************************************************/
    public void EnableHovor(int currentSkillSlot)
    {
        infoWindow[currentSkillSlot].SetActive(true);
        SetSkillInfoWindow(currentSkillSlot);
    }
    public void DisableHovor(int currentSkillSlot)
    {
        if(currentSkillSlot != -1)
            infoWindow[currentSkillSlot].SetActive(false);
    }

    /**********************************************************
    * ��ų����â ����
    ***********************************************************/
    private void SetSkillInfoWindow(int skillNum)
    {
        var skillData = Turn.unit.skills[skillNum].GetComponent<Skill>().data;

        skillWindowInfo[skillNum].skillName.text = skillData.name;
        skillWindowInfo[skillNum].coolTime.text = skillData.coolTime.ToString();
        skillWindowInfo[skillNum].type.text = skillData.damageType.ToString();
        skillWindowInfo[skillNum].range.text = skillData.range.ToString();

        if(skillData.isAOE == true)
        {
            skillWindowInfo[skillNum].target.text = "����";
        }
        else
        {
            skillWindowInfo[skillNum].target.text = "����";
        }

        string explainText = skillData.explain.Replace("{multiplier}", skillData.multiplier.ToString());
        skillWindowInfo[skillNum].info.text = explainText;

        //skillWindowInfo[skillNum].info.text = skillData.explain.ToString();
    }
}
