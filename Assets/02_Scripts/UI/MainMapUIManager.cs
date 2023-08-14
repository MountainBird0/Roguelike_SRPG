/**********************************************************
* MainMapScene�� UI ����
***********************************************************/
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMapUIManager : MonoBehaviour
{
    public TextMeshProUGUI seed;

    public ImagePool pool;

    public GameObject unitSlot;

    private List<GameObject> skillSlots = new();

    public StatWindow statWindow;

    [Header("Transform")]
    public Transform unitContent;
    public Transform skillContent;

    private void Awake()
    {
        seed.text = DataManager.instance.gameInfo.seed.ToString();
        pool.MakeDictionarys();
    }

    public void CreateUnitSlot(MainMapUIController controller)
    {
        foreach (var kvp in DataManager.instance.currentUnitInfo)
        {
            var ob = Instantiate(unitSlot, unitContent);
            ob.name = kvp.Key;
            ob.GetComponent<UnitSlot>().icon.sprite = pool.smallImages[kvp.Key];

            controller.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }

        controller.InitializeButtons();
    }

    /**********************************************************
    * ��ų���� ���� / ����
    ***********************************************************/
    public void CreateSkillSlot(string unitName)
    {
        GameObject ob;

        var UnitSkills = DataManager.instance.currentUnitSkills;
        if(UnitSkills.ContainsKey(unitName))
        {
            for (int i = 0; i < UnitSkills[unitName].list.Count; i++)
            {
                ob = ObjectPoolManager.instance.Spawn("SkillSlot");
                ob.transform.SetParent(skillContent);

                var defaultSkills = DataManager.instance.defaultSkills;
                if(defaultSkills.ContainsKey(UnitSkills[unitName].list[i]))
                {
                    var skillName = defaultSkills[UnitSkills[unitName].list[i]].name;
                    if (pool.SkillImages.ContainsKey(skillName))
                    {
                        ob.GetComponent<SkillSlot>().icon.sprite = pool.SkillImages[skillName];
                    }
                }
                skillSlots.Add(ob);
            }
        }
    }
    public void ClearSkillSlot()
    {
        for(int i = 0; i < skillSlots.Count; i++)
        {
            ObjectPoolManager.instance.Despawn(skillSlots[i]);
        }
        skillSlots.Clear();
    }

    /**********************************************************
    * ���� ������ ����
    ***********************************************************/
    public void SetStatWindow(string unitName)
    {
        var statData = DataManager.instance.currentUnitInfo[unitName];
        
        statWindow.className.text = unitName;
        statWindow.level.text = statData.Level.ToString();
        statWindow.atk.text   = statData.ATK.ToString();
        statWindow.def.text   = statData.DEF.ToString();
        statWindow.matk.text  = statData.MATK.ToString();
        statWindow.mdef.text  = statData.MDEF.ToString();
        statWindow.hit.text   = statData.HIT.ToString();
        statWindow.eva.text   = statData.EVA.ToString();
        statWindow.cri.text   = statData.CRI.ToString();
        statWindow.res.text   = statData.RES.ToString();
        statWindow.mov.text   = statData.MOV.ToString();
        statWindow.speed.text = statData.SPEED.ToString();
    }


}
