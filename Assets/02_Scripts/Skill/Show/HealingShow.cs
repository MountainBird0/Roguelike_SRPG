using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingShow : SkillVisualEffect
{
    private string smallEffectName = "Heal";
    private float delay = 1.0f;

    public override void Apply(SkillEffect effect)
    {
        StartCoroutine(HealApply(effect));
    }

    public override float GetDuration()
    {
        return delay;
    }
    private IEnumerator HealApply(SkillEffect effect)
    {
        foreach (var target in Turn.targets)
        {
            GameObject ob = ObjectPoolManager.instance.Spawn(smallEffectName);
            ob.transform.position = target.pos;
        }

        yield return new WaitForSeconds(delay);

        effect.Apply();
    }
}
