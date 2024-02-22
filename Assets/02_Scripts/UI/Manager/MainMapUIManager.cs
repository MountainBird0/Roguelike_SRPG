/**********************************************************
* MainMap�� UI ����, ���� ��
***********************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class MainMapUIManager : MonoBehaviour
{
    public static MainMapUIManager instance;

    public TextMeshProUGUI seed;

    public GameObject unitSlot;

    public StatInfo statInfo;
    public SkillInfo skillInfo;

    [Header("Transform")]
    public Transform unitContent;
    public Transform skillContent;

    public Dictionary<int, GameObject> skillSlots = new();

    [Header("ImagePool")]
    public StringKeyImagePool unitSmallPool;
    public StringKeyImagePool unitBigPool;
    public IntKeyImagePool skillIconPool;

    [Header("DefaultImage")]
    public Sprite defaultSprite;

    private void Awake()
    {       
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
        
        seed.text = DataManager.instance.gameInfo.seed.ToString();
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


            slot.icon.sprite = unitSmallPool.images[kvp.Key];

            slot.name = kvp.Key;

            controller.unitButtons.Add(slot.name, ob.GetComponent<Button>());
        }

        controller.InitializeUnitButtons();
    }

    /**********************************************************
    * ��ų���� ���� / ����
    ***********************************************************/
    public async void CreateSkillSlot(string unitName, List<GameObject> equipSkillSlots)
    {
        ClearSkillSlot();

        await CreateUsableSkills(unitName);
        await CreateEquippedSkills(unitName, equipSkillSlots);
    }
    /**********************************************************
    * ��밡���� ��ü ��ų���� ����
    ***********************************************************/
    private async Task CreateUsableSkills(string name)
    {
        GameObject ob;
        var usableSkills = DataManager.instance.currentUsableSkills[name];
        for(int i = 0; i < usableSkills.Count; i++)
        {
            int skillId = usableSkills[i];

            ob = ObjectPoolManager.instance.Spawn("SkillSlot");
            ob.transform.SetParent(skillContent);
            ob.transform.localScale = new Vector3(1f, 1f, 1f); // ���� �ʿ�

            var slot = ob.GetComponent<SkillSlot>();
            slot.image.sprite = await AddressableManager.instance.GetImage(skillId.ToString());
            slot.id = skillId;
            slot.check.SetActive(false);

            skillSlots.Add(skillId, ob);
        }
    }
    /**********************************************************
    * ��ų ����ĭ�� ��ų���� ����
    ***********************************************************/
    private async Task CreateEquippedSkills(string name, List<GameObject> equipSkillSlots)
    {
        var equipSkills = DataManager.instance.currentEquipSkills[name];
        for(int i = 0; i < equipSkillSlots.Count; i++)
        {
            var slot = equipSkillSlots[i].GetComponent<SkillSlot>();
            
            if(equipSkills.Count <= i)
            {
                slot.id = -1; 
                slot.image.sprite = defaultSprite;
                continue;
            }

            var SkillId = equipSkills[i];

            slot.image.sprite = await AddressableManager.instance.GetImage(SkillId.ToString());
            slot.id = SkillId;

            if(skillSlots.ContainsKey(SkillId))
            {
                skillSlots[SkillId].GetComponent<SkillSlot>().check.SetActive(true);
            }
        }
    }


    /**********************************************************
    * ��ų ���� ����
    ***********************************************************/
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

        statInfo.hp.text = statData.HP.ToString() + " / " + statData.MaxHP.ToString();
        float hpRatio = (float)statData.HP / statData.MaxHP;
        statInfo.redBar.fillAmount = hpRatio;

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
    public void SetSkillInfoWindow(int skillNum)
    {
        if(DataManager.instance.defaultSkillStats.ContainsKey(skillNum))
        {
            var skillData = DataManager.instance.defaultSkillStats[skillNum];

            skillInfo.skillName.text = skillData.name;
            skillInfo.coolTime.text = skillData.coolTime.ToString();
            skillInfo.type.text = skillData.damageType.ToString();
            skillInfo.range.text = skillData.range.ToString();
            skillInfo.range.text = skillData.range.ToString();

            if (skillData.isAOE == true)
            {
                skillInfo.target.text = "����";
            }
            else
            {
                skillInfo.target.text = "����";
            }

            skillInfo.explain.text = skillData.explain.Replace("{multiplier}", skillData.multiplier.ToString());
        }
    }
}
