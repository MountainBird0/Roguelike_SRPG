using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBallShow : SkillVisualEffect
{
    private float delay = 1.1f;

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
