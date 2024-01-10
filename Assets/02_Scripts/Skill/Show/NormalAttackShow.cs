using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackShow : SkillVisualEffect
{
    private float delay = 0.7f;

    public override void Apply(SkillEffect effect)
    {
        transform.position = Turn.targets[0].transform.position;
        effect.Apply();
    }

    public override float GetDuration()
    {
        return delay;
    }

}
