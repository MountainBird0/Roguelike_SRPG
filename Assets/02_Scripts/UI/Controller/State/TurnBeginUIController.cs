using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnBeginUIController : MonoBehaviour
{
    public Canvas turnBeginCanvas;

    public GameObject statWindow;

    public Image unitIcon;
    public StatInfo statInfo;

    public void DisableCanvas()
    {
        turnBeginCanvas.gameObject.SetActive(false);
    }

    // ½º¸ôÇ®»ç¿ë
    public void ShowStatWindow(Unit unit)
    {
        turnBeginCanvas.gameObject.SetActive(true);
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
    }
}
