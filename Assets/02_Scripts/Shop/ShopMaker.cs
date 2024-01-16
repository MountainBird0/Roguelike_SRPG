using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopMaker : MonoBehaviour
{
    // ��ų ��ϵ�
    // Ȯ�� ������ ���� �� �ִ� ���� ��� ����
    // �׳� x��ư


    public Transform unitContent;

    public List<SelectableSkillWindow> skillWindows;

    public ToggleGroup TG;

    private void OnEnable()
    {
        CreateSelectalbeSkill();
    }

    /**********************************************************
    * ��ų 2�� ����
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
            window.target.text = "����";
        }
        else
        {
            window.target.text = "����";
        }

        string explainText = data.explain.Replace("{multiplier}", data.multiplier.ToString());
        window.explain.text = explainText;
    }
}
