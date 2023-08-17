using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public string name;
    public string explain;
    public int range;
    public int verticalRange;
    public int AOERange;
    public int AOEVerRange;
    public int affectType;
    public int jobType;
    public int damageType;
    public float multiplier;
    // 공격대상
    // public affectType Enemy; // enum
}
//affectType
// 0 - Default
// 1 - A
// 2 - Enemy