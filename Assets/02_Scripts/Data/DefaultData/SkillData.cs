using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public string name;
    public string explain;
    public int range;
    // public int verticalRange;
    public int AOERange;
    // public int AOEVerRange;
    public int affectType;
    public int jobType;
    public int rangeType;
    public int damageType;
    public float multiplier;
    // 공격대상
    // public affectType Enemy; // enum
}
// affectType
// 0 - Default
// 1 - A
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