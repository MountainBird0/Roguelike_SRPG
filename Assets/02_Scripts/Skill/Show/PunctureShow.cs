using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunctureShow : SkillVisualEffect
{
    public GameObject swordEffect;

    private string smallEffectName = "SwordBlock";

    public override void Apply(SkillEffect effect)
    {
        sequence = DOTween.Sequence()
            .Append(swordEffect.transform.DOLocalMove(Turn.direction * Turn.skill.data.range, 0.2f)
                .From(Turn.unit.pos)).SetRelative().SetEase(Ease.OutQuad).SetLoops(2, LoopType.Yoyo)
                    .OnComplete(() =>
                    {
                        effect.Apply();
                    });

        StartCoroutine(AttackApply());
    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }

    private IEnumerator AttackApply()
    {
        List<GameObject> obList = new();

        foreach (var target in Turn.targets)
        {
            GameObject ob = ObjectPoolManager.instance.Spawn(smallEffectName);
            ob.transform.position = target.pos;
            obList.Add(ob);
        }

        yield return new WaitForSeconds(1.2f);

        for (int i = 0; i < obList.Count; i++)
        {
            ObjectPoolManager.instance.Despawn(obList[i]);
        }
    }
}
