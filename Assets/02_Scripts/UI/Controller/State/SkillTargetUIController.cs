using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetUIController : MonoBehaviour
{
    public Canvas skillTargetCanvas;

    public GameObject ChooseList;

    [Header("window")]
    public GameObject battleWindow;
    public GameObject rightOb;
    public GameObject leftOb;
    public GameObject rightSkillSprite;
    public GameObject leftSkillSprite;
    public TargetingWindow rightWindow;
    public TargetingWindow leftWindow;


    /**********************************************************
    * 취소 버튼
    ***********************************************************/
    public void ClickBtnCancel()
    {
        Debug.Log($"{GetType()} - 취소 누름");
        if (Turn.skill.data.isDirectional)
        {
            StateMachineController.instance.ChangeTo<ArrowSelectionState>();
            return;
        }
        StateMachineController.instance.ChangeTo<SkillSelectedState>();
    }


    /**********************************************************
    * 공격확인 버튼
    ***********************************************************/
    public void ClickBtnAttack()
    {
        Debug.Log($"{GetType()} - 공격 누름");
        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


    /**********************************************************
    * 선택 캔버스 활성화/비활성화
    ***********************************************************/
    public void EnableCanvas()
    {
        skillTargetCanvas.gameObject.SetActive(true);
        ChooseList.transform.DORotate(new Vector3(0, 0, 150), 0.2f).From(true);

        if (Turn.skill.data.isAOE == false)
        {
            EnableBattleWindow();
        }

    }
    public void DisableCanvas()
    {
        skillTargetCanvas.gameObject.SetActive(false);
        battleWindow.SetActive(false);
    }

    /**********************************************************
    * 정보 윈도우 활성화
    ***********************************************************/
    public void EnableBattleWindow()
    {
        battleWindow.SetActive(true);
        var rObrt = rightOb.GetComponent<RectTransform>();
        rObrt.DOAnchorPosX(50, 0.5f).From(new Vector2(-150, rObrt.anchoredPosition.y));
        var lObrt = leftOb.GetComponent<RectTransform>();
        lObrt.DOAnchorPosX(-50, 0.5f).From(new Vector2(150, lObrt.anchoredPosition.y));

        SettingWindow();
    }

    /**********************************************************
    * 정보 윈도우 내부세팅
    ***********************************************************/
    private void SettingWindow()
    {
        Unit rightUnit;
        Unit leftUnit;

        if (Turn.isHumanTurn)
        {
            rightUnit = Turn.unit;
            leftUnit = Turn.targets[0];

            rightSkillSprite.SetActive(true);
            leftSkillSprite.SetActive(false);
            rightWindow.skillImage.sprite = Turn.skill.image;
        }
        else
        {
            rightUnit = Turn.targets[0];
            leftUnit = Turn.unit;

            rightSkillSprite.SetActive(false);
            leftSkillSprite.SetActive(true);
            leftWindow.skillImage.sprite = Turn.skill.image;
        }

        SettingWindow(rightWindow, rightUnit);
        SettingWindow(leftWindow, leftUnit);
    }
    /**********************************************************
    * 세부세팅
    ***********************************************************/
    private void SettingWindow(TargetingWindow window, Unit unit)
    {
        window.unitImage.sprite = unit.image;
        window.className.text = unit.unitName;

        var data = unit.stats;
        window.level.text = "Lv." + data.Level.ToString();
        window.hp.text = data.HP + " / " + data.MaxHP;

        if(Turn.skill.data.damageType == DamageType.PHYSICS)
        {
            window.attack.text = "ATK : " + data.ATK.ToString();
            window.defensive.text = "DEF : " + data.DEF.ToString();
        }
        else
        {
            window.attack.text = "MATK : " + data.MATK.ToString();
            window.defensive.text = "MDEF : " + data.MDEF.ToString();
        }

        float hpRatio = (float)data.HP / data.MaxHP;
        window.redBar.fillAmount = hpRatio;
    }


}
