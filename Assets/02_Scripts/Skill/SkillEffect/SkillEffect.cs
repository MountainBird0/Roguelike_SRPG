using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffect : MonoBehaviour
{
    public float delay = 0f;
    public string effectName = "";    
    public abstract void Apply();
}
