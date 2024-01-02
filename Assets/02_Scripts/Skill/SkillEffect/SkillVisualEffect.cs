using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillVisualEffect : MonoBehaviour
{
    protected Sequence sequence;

    public abstract void Apply(SkillEffect effect);

    public abstract float GetDuration();
}
