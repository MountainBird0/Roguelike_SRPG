using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Sprite image;

    [HideInInspector]
    public int coolTime;
    [HideInInspector]
    public int id;

    public void SetCoolTime(int coolTime)
    {
        this.coolTime = coolTime;
    }

    public void ReduceCoolTime()
    {
        coolTime -= 1;
    }
}
