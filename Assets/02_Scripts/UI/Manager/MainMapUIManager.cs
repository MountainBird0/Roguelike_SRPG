/**********************************************************
* MainMap�� UI ����, ���� ��
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

    public StatInfo statInfo;
    public SkillInfo skillInfo;

    [Header("Transform")]
    public Transform unitContent;
    public Transform skillContent;

    public Dictionary<int, GameObject> skillSlots = new();    
    
    private void Awake()
    {
        seed.text = DataManager.instance.gameInfo.seed.ToString();
        pool.MakeDictionarys();
    }

    /**********************************************************
    * ���ֽ��� ����
    ***********************************************************/
    public void CreateUnitSlot(MainMapUIController controller)
    {
        foreach (var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(unitSlot, unitContent);
            // ob.name = kvp.Key;
            var slot = ob.GetComponent<UnitSlot>();
            slot.icon.sprite = pool.smallImages[kvp.Key];
            slot.name = kvp.Key;

            controller.unitButtons.Add(slot.name, ob.GetComponent<Button>());
        }

        controller.InitializeUnitButtons();
    }

    /**********************************************************
    * ��ų���� ���� / ����
    ***********************************************************/
    public void CreateSkillSlot(string unitName, List<GameObject> equipSkillSlots)
    {
        ClearSkillSlot();

        // ��ü ��밡���� ��ų ���̰�
        GameObject ob;
        var usableSkills = DataManager.instance.currentUsableSkills[unitName];
        for(int i = 0; i < usableSkills.list.Count; i++)
        {    
            var SkillId = usableSkills.list[i];
            if (pool.skillImages.ContainsKey(SkillId)) // �̸����� �̹��� ã��
            {
                ob = ObjectPoolManager.instance.Spawn("SkillSlot");
                ob.transform.SetParent(skillContent);
                ob.transform.localScale = new Vector3(1f, 1f, 1f); // ����

                var slot = ob.GetComponent<SkillSlot>();
                slot.image.sprite = pool.skillImages[SkillId];
                slot.id = SkillId;
                slot.check.SetActive(false);
               
                skillSlots.Add(SkillId, ob);
            }          
        }

        // ������ ��ų ���̰�
        var equipSkills = DataManager.instance.currentEquipSkills[unitName];
        for (int i = 0; i < equipSkills.list.Count; i++)
        {
            var SkillId = equipSkills.list[i];
            if (skillSlots.ContainsKey(SkillId))
            {
                var slot = equipSkillSlots[i].GetComponent<SkillSlot>();
                slot.image.sprite = pool.skillImages[SkillId];
                slot.id = SkillId;

                skillSlots[SkillId].GetComponent<SkillSlot>().check.SetActive(true);
            }
            else
            {
                var slot = equipSkillSlots[i].GetComponent<SkillSlot>();
                slot.image.sprite = pool.skillImages[SkillId];
                slot.id = SkillId;
            }
        }
    }
    private void ClearSkillSlot()
    {
        foreach(var kvp in skillSlots)
        {
            ObjectPoolManager.instance.Despawn(kvp.Value);
        }
        skillSlots.Clear();
    }

    /**********************************************************
    * ���� ������ ����
    ***********************************************************/
    public void SetStatWindow(string unitName)
    {
        var statData = DataManager.instance.currentUnitStats[unitName];
        
        statInfo.className.text = unitName;
        statInfo.level.text = statData.Level.ToString();
        statInfo.atk.text   = statData.ATK.ToString();
        statInfo.def.text   = statData.DEF.ToString();
        statInfo.matk.text  = statData.MATK.ToString();
        statInfo.mdef.text  = statData.MDEF.ToString();
        statInfo.hit.text   = statData.HIT.ToString();
        statInfo.eva.text   = statData.EVA.ToString();
        statInfo.cri.text   = statData.CRI.ToString();
        statInfo.res.text   = statData.RES.ToString();
        statInfo.mov.text   = statData.MOV.ToString();
        statInfo.speed.text = statData.SPEED.ToString();
    }

    /**********************************************************
    * ��ų ������ ����
    ***********************************************************/
    public void SetSkillWindow(int skillNum)
    {
        if(DataManager.instance.defaultSkillStats.ContainsKey(skillNum))
        {
            var skillData = DataManager.instance.defaultSkillStats[skillNum];

            skillInfo.skillName.text = skillData.name;
            skillInfo.explain.text = skillData.explain;
        }
    }
}
