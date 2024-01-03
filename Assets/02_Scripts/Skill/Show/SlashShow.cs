using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashShow : SkillVisualEffect
{
    private string smallEffectName = "Stone";
    private float delay = 2.5f;

    public override void Apply(SkillEffect effect)
    {
        StartCoroutine(StoneApply(effect));     
    }

    public override float GetDuration()
    {
        return delay;
    }
    private IEnumerator StoneApply(SkillEffect effect)
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
