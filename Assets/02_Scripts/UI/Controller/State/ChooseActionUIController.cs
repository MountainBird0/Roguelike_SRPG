/**********************************************************
* ChooseActionState�� UI ��Ʈ��
***********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseActionUIController : MonoBehaviour
{
    public Canvas actionCanvas;
    public GraphicRaycaster raycaster;

    public ImagePool imagePool;

    public List<GameObject> skillSlots;

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
    }
    public void DisableCanvas()
    {
        actionCanvas.gameObject.SetActive(false);
    }

    /**********************************************************
    * �÷��̾� �� ��ų ����
    ***********************************************************/
    private void SetSkillIcon()
    {
        var skill = DataManager.instance.currentEquipSkills[Turn.unit.unitName];

        for(int i = 0; i < skillSlots.Count; i++)
        {
            var id = skill.list[i];
            var slotInfo = skillSlots[i].GetComponent<SkillSlot>();
            slotInfo.id = id;
            slotInfo.image.sprite = imagePool.skillImages[id];
        }
    }

    /**********************************************************
    * ��ų ������ ����
    ***********************************************************/
    public void ClickBtnSkill()
    {
        StateMachineController.instance.ChangeTo<SkillSelectionState>();
    }


}
