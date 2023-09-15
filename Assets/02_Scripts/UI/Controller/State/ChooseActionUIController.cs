/**********************************************************
* ChooseActionState의 UI 컨트롤
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

    [Header("StatWindow")]
    public Image unitImage;
    public Image redBar;
    public TextMeshProUGUI className;
    public TextMeshProUGUI level;
    public TextMeshProUGUI hp;

    private StringKeyImagePool unitBigPool;
    private IntKeyImagePool skillIconPool;

    private void Start()
    {
        // awake에 넣으면 안댐 순서 신경쓰기
        //unitImagePool = BattleMapUIManager.instance.unitbt
        skillIconPool = BattleMapUIManager.instance.skillIconPool;
        unitBigPool = BattleMapUIManager.instance.unitBigPool;
    }

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
        SetStatWindow();
    }
    public void DisableCanvas()
    {
        actionCanvas.gameObject.SetActive(false);
    }

    /**********************************************************
    * 스킬 세팅
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
    * 스탯윈도우 세팅
    ***********************************************************/
    private void SetStatWindow()
    {
        var unitName = Turn.unit.unitName;
        unitImage.sprite = unitBigPool.images[unitName];
        className.text = unitName;

        var statInfo = Turn.unit.stats;
        level.text = statInfo.Level.ToString();
        hp.text = statInfo.HP.ToString() + " / " + statInfo.MaxHP.ToString();

        float hpRatio = (float)statInfo.HP / statInfo.MaxHP;
        redBar.fillAmount = hpRatio;
    }

    public void ClickBtnttt()
    {
        // 테스트좀하자
        Debug.Log($"{GetType()} - 버튼은 눌림");
    }
}
