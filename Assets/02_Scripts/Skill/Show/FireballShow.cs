using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireballShow : SkillVisualEffect
{
    public GameObject ballEffect;

    public override void Apply(SkillEffect effect)
    {
        // 터지는 이펙트 추가

        sequence = DOTween.Sequence().
            Append(ballEffect.transform.DOMove(Turn.selectedPos, 1.0f).From(Turn.unit.pos))
                .OnComplete(() => effect.Apply());

    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }
}
