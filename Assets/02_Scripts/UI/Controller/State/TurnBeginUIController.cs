using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnBeginUIController : MonoBehaviour
{
    public Canvas turnBeginCanvas;

    public GameObject statWindow;

    public Image unitIcon;
    public StatInfo statInfo;

    [Header("SkillSlot")]
    public List<GameObject> skillSlots;
    public List<GameObject> coolTimeImage;
    public List<TextMeshProUGUI> coolTimeText;

    public void DisableCanvas()
    {
        turnBeginCanvas.GetComponent<CanvasGroup>().DOFade(0f, 0.3f).OnComplete(() =>
        {
            turnBeginCanvas.gameObject.SetActive(false);
        });
    }

    // ½º¸ôÇ®»ç¿ë
    public void ShowStatWindow(Unit unit)
    {
        turnBeginCanvas.gameObject.SetActive(true);
        turnBeginCanvas.GetComponent<CanvasGroup>().alpha = 1f;
        unitIcon.sprite = unit.image;

        var statData = unit.stats;
        statInfo.className.text = unit.unitName;
        statInfo.level.text = statData.Level.ToString();
        statInfo.hp.text = statData.HP.ToString() + " / " + statData.MaxHP.ToString();
        statInfo.atk.text = statData.ATK.ToString();
        statInfo.def.text = statData.DEF.ToString();
        statInfo.matk.text = statData.MATK.ToString();
        statInfo.mdef.text = statData.MDEF.ToString();
        statInfo.hit.text = statData.HIT.ToString();
        statInfo.eva.text = statData.EVA.ToString();
        statInfo.cri.text = statData.CRI.ToString();
        statInfo.res.text = statData.RES.ToString();

        float hpRatio = (float)statData.HP / statData.MaxHP;
        statInfo.redBar.fillAmount = hpRatio;

        for (int i = 0; i < skillSlots.Count; i++)
        {
            var slot = skillSlots[i].GetComponent<BattleSkillSlot>();

            if (unit.skills.Count <= i)
            {
                Debug.Log($"{GetType()} - ¿©±âºö");
                slot.slotNum = -1;
                slot.image.sprite = BattleMapUIManager.instance.defaultSprite;
                continue;
            }

            var skill = unit.skills[i].GetComponent<Skill>();

            slot.slotNum = i;
            slot.id = skill.id;
            slot.image.sprite = skill.image;

            if(skill.data.currentCoolTime > 0)
            {
                coolTimeImage[i].SetActive(true);
                coolTimeText[i].text = skill.data.currentCoolTime.ToString();
            }
            else
            {
                coolTimeImage[i].SetActive(false);
            }
        }
    }
}
