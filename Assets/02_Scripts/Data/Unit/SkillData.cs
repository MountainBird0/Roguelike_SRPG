/**********************************************************
* 각 스킬의 정보 저장
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
    // 공격대상
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
// 0 - 물리
// 1 - 마법

// rangeType
// 0 - constant
// 1 - line
// 2 - cone
// 3 - infinite
// 4 - self