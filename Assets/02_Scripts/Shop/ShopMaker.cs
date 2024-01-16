using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopMaker : MonoBehaviour
{
    // 스킬 목록들
    // 확인 누르면 넣을 수 있는 유닛 목록 등장
    // 그냥 x버튼


    public Transform unitContent;

    public List<SelectableSkillWindow> skillWindows;

    public ToggleGroup TG;

    private void OnEnable()
    {
        CreateSelectalbeSkill();
    }

    /**********************************************************
    * 스킬 2개 생성
    ***********************************************************/
    public async Task CreateSelectalbeSkill()
    {
        for(int i = 0; i < 2; i++)
        {
            List<int> shopSkills = DataManager.instance.shopSkills;

            int num = Random.Range(0, shopSkills.Count);
            SkillData skillData = DataManager.instance.defaultSkillStats[shopSkills[num]];

            var window = skillWindows[i];

            window.skillNum = shopSkills[num];
            window.skillImage.sprite = await AddressableManager.instance.GetImage(shopSkills[num].ToString());
            SettingWindow(window, skillData);

            DataManager.instance.shopSkills.RemoveAt(num);
        }
    }

    public void GetSkill()
    {
        var toggles = TG.GetComponentsInChildren<Toggle>(true);

        for(int i = 0; i < toggles.Length; i++)
        {
            if(toggles[i].isOn)
            {
                DataManager.instance.currentUsableSkills[skillWindows[i].unitName.text].Add(skillWindows[i].skillNum);
                break;
            }
        }
    }




    private void SettingWindow(SelectableSkillWindow window, SkillData data)
    {
        if (data.jobType == 0)
        {
            window.unitName.text = "Warrior";
        }
        else if (data.jobType == 1)
        {
            window.unitName.text = "Mage";
        }
        else
        {
            window.unitName.text = "Rogue";
        }

        window.skillName.text = data.name;
        window.coolTime.text = data.coolTime.ToString();
        window.type.text = data.damageType.ToString();
        window.range.text = data.range.ToString();

        if (data.isAOE == true)
        {
            window.target.text = "범위";
        }
        else
        {
            window.target.text = "단일";
        }

        string explainText = data.explain.Replace("{multiplier}", data.multiplier.ToString());
        window.explain.text = explainText;
    }
}
