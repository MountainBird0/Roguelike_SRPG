/**********************************************************
* �� ��ų�� ���� ����
***********************************************************/
public class SkillData
{
    public string name;
    public string explain;
    public int range;
    // public int verticalRange;
    public int AOERange;
    // public int AOEVerRange;
    public string affectType;
    public int jobType;
    public string rangeType;
    public string damageType;
    public float multiplier;
    public bool isDirectional;
    // ���ݴ��
    // public affectType Enemy; // enum
}
// affectType
// 0 - Default
// 1 - ALLY
// 2 - Enemy

// jobType
// 0 - warrior
// 1 - mage
// 2 - rogue
// 4 - archer

// damageType
// 0 - ����
// 1 - ����

// rangeType
// 0 - constant
// 1 - line
// 2 - cone
// 3 - infinite
// 4 - self