using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashShow : SkillVisualEffect
{
    private string smallEffectName = "Stone";
    private float delay = 4.0f;

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
        List<GameObject> obList = new();

        foreach (var target in Turn.targets)
        {
            GameObject ob = ObjectPoolManager.instance.Spawn(smallEffectName);
            ob.transform.position = target.pos;
            obList.Add(ob);
        }

        yield return new WaitForSeconds(2.5f);

        effect.Apply();

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < obList.Count; i++)
        {
            ObjectPoolManager.instance.Despawn(obList[i]);
        }
    }
}
