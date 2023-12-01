using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Sprite image;
    public List<SkillEffect> effects;

    [HideInInspector]
    public int id;

    public SkillData data;

    public void SetCoolTime(int coolTime)
    {
        Debug.Log($"{GetType()} - ���");
        data.currentCoolTime = data.coolTime;
    }

    public void ReduceCoolTime()
    {
        Debug.Log($"{GetType()} - �̰� ���");
        data.currentCoolTime -= 1;
    }
}
