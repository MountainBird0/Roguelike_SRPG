/**********************************************************
* MainMapScene의 UI 관리, ui생성 등
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

    private List<GameObject> skillSlots = new();
    private Dictionary<int, GameObject> currentSkills = new();    
    
    private void Awake()
    {
        seed.text = DataManager.instance.gameInfo.seed.ToString();
        pool.MakeDictionarys();
    }

    /**********************************************************
    * 유닛슬롯 생성
    ***********************************************************/
    public void CreateUnitSlot(MainMapUIController controller)
    {
        foreach (var kvp in DataManager.instance.currentUnitInfo)
        {
            var ob = Instantiate(unitSlot, unitContent);
            ob.name = kvp.Key;
            ob.GetComponent<UnitSlot>().icon.sprite = pool.smallImages[kvp.Key];

            controller.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }

        controller.InitializeUnitButtons();
    }

    /**********************************************************
    * 스킬슬롯 생성 / 삭제
    ***********************************************************/
    public void CreateSkillSlot(string unitName, List<GameObject> skillSet)
    {
        ClearSkillSlot();

        GameObject slot;

        var defaultSkills = DataManager.instance.defaultSkills; // 전체 스킬 dic

        // 슬롯 스킬
        var slotSkills = DataManager.instance.currentSlotSkills[unitName];
        for (int i = 0; i < slotSkills.list.Count; i++)
        {
            // 이미지 세팅
            skillSet[i].GetComponent<SkillSlot>().image.sprite = pool.skillImages[slotSkills.list[i]];
            skillSet[i].GetComponent<SkillSlot>().id = slotSkills.list[i];
            // skillSet[i].name = slotSkills.list[i].ToString();
        }


        // 전체 스킬
        var unitSkills = DataManager.instance.currentUnitSkills[unitName]; // 현재 유닛의 사용가능 스킬 리스트
        for(int i = 0; i < unitSkills.list.Count; i++)
        {
            slot = ObjectPoolManager.instance.Spawn("SkillSlot");
            slot.transform.SetParent(skillContent);
            slot.transform.localScale = new Vector3(1f, 1f, 1f); // 수정
      
            if (defaultSkills.ContainsKey(unitSkills.list[i]))      // 번호로 스킬 찾음
            {
                var skillName = unitSkills.list[i];
                if (pool.skillImages.ContainsKey(skillName)) // 이름으로 이미지 찾음
                {
                    var slotInfo = slot.GetComponent<SkillSlot>();
                    slotInfo.icon.sprite = pool.skillImages[skillName];
                    slotInfo.imageSlot.name = unitSkills.list[i].ToString();
                    if(i < slotSkills.list.Count)
                    {
                        slotInfo.check.SetActive(true);
                    }
                }
            }
            skillSlots.Add(slot);
            currentSkills.Add(unitSkills.list[i], slot);
        }
    }
    private void ClearSkillSlot()
    {
        for(int i = 0; i < skillSlots.Count; i++)
        {
            skillSlots[i].GetComponent<SkillSlot>().check.gameObject.SetActive(false);
            ObjectPoolManager.instance.Despawn(skillSlots[i]);
        }
        skillSlots.Clear();

        foreach(var kvp in currentSkills)
        {

        }
        currentSkills.Clear();
    }

    /**********************************************************
    * 스탯 윈도우 세팅
    ***********************************************************/
    public void SetStatWindow(string unitName)
    {
        var statData = DataManager.instance.currentUnitInfo[unitName];
        
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
    * 스킬 윈도우 세팅
    ***********************************************************/
    public void SetSkillWindow(int skillNum)
    {
        var skillData = DataManager.instance.defaultSkills[skillNum];

        skillInfo.skillName.text = skillData.name;
        skillInfo.explain.text = skillData.explain;
    }

}
