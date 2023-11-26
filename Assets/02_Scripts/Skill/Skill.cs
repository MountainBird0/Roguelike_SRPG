using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Sprite image;
    public List<SkillEffect> effects;

    [HideInInspector]
    public int coolTime;
    [HideInInspector]
    public int id;

    public SkillData data;

    public void SetCoolTime(int coolTime)
    {
        data.currentCoolTime = data.coolTime;
    }

    public void ReduceCoolTime()
    {
        data.currentCoolTime -= 1;
    }
}
