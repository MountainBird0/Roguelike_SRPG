/**********************************************************
* ChooseActionState�� UI ��Ʈ��
***********************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseActionUIController : MonoBehaviour
{
    public Canvas actionCanvas;
    public GraphicRaycaster raycaster;
    
    [Header("SkillSlot")]
    public List<GameObject> skillSlots;
    public List<GameObject> coolTimeImage;
    public List<TextMeshProUGUI> coolTimeText;

    public StatInfo statInfo;

    [Header("StatWindow")]
    public Image unitImage;


    private StringKeyImagePool unitBigPool;
    private IntKeyImagePool skillIconPool;

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
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableCanvas()
    {
        actionCanvas.gameObject.SetActive(true);
        SetSkillIcon();
        SetStatWindow();
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
    }

    /**********************************************************
    * ��ų ����
    ***********************************************************/
    private void SetSkillIcon()
    {
        var skill = DataManager.instance.currentEquipSkills[Turn.unit.unitName];

        for(int i = 0; i < skillSlots.Count; i++)
        {
            var id = skill.list[i];
            //var slotInfo = skillSlots[i].GetComponent<SkillSlot>();
            var slotInfo = skillSlots[i].GetComponent<BattleSkillSlot>();
            slotInfo.slotNum = i;
            slotInfo.id = id;
            slotInfo.image.sprite = skillIconPool.images[id];
        }
    }

    /**********************************************************
    * ��Ÿ�� ����
    ***********************************************************/
    public void SetCoolTime(int slotNum)
    {
        for(int i = 0; i < coolTimeImage.Count; i++)
        {
            if(slotNum.Equals(i))
            {
                if(Turn.currentSkill.coolTime != 0)
                {
                    coolTimeImage[i].SetActive(true);
                    coolTimeText[i].text = Turn.currentSkill.coolTime.ToString();
                }
            }
            else if(coolTimeImage[i].activeSelf)
            {
                int currentCoolTime = int.Parse(coolTimeText[i].text);
                currentCoolTime -= 1;
                if(currentCoolTime <= 0)
                {
                    coolTimeImage[i].SetActive(false);
                }
                else
                {
                    coolTimeText[i].text = currentCoolTime.ToString();
                }
            }
        }
    }


    public void ClickBtnttt()
    {
        // �׽�Ʈ������
        Debug.Log($"{GetType()} - ��ư�� ����");
    }
}
