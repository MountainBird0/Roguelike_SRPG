using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundClawShow : SkillVisualEffect
{
    private float delay = 4.0f;

    public override void Apply(SkillEffect effect)
    {
        StartCoroutine(ClawApply(effect));
    }

    public override float GetDuration()
    {
        return delay;
    }

    private IEnumerator ClawApply(SkillEffect effect)
    {
        var pos = Turn.selectedPos;

        transform.position = new Vector3Int(pos.x, pos.y, 5);

        yield return new WaitForSeconds(3.0f);

        effect.Apply();
    }
}
