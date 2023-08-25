/**********************************************************
* ChooseActionState의 UI 컨트롤
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
    * 턴 종료 버튼
    ***********************************************************/
    public void ClickBtnEnd()
    {
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * 선택 캔버스 활성화/비활성화
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
    * 플레이어 별 스킬 세팅
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
    * 스킬 아이콘 누름
    ***********************************************************/
    public void ClickBtnSkill()
    {
        StateMachineController.instance.ChangeTo<SkillSelectionState>();
    }


}
