using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireballShow : SkillVisualEffect
{
    public GameObject ballEffect;

    public override void Apply(SkillEffect effect)
    {
        // ������ ����Ʈ �߰�

        sequence = DOTween.Sequence().
            Append(ballEffect.transform.DOMove(Turn.selectedPos, 1.0f).From(Turn.unit.pos))
                .OnComplete(() => effect.Apply());

    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }
}
