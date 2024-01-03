using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PunctureShow : SkillVisualEffect
{
    public GameObject swordEffect;

    public override void Apply(SkillEffect effect)
    {
        sequence = DOTween.Sequence()
            .Append(swordEffect.transform.DOMove(Turn.selectedPos, 0.1f).From(Turn.unit.pos)).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    effect.Apply();
                })
            .Append(swordEffect.transform.DOMove(Turn.unit.pos, 0.2f).SetEase(Ease.OutQuad));
    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }
}
