using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireballShow : SkillVisualEffect
{
    public GameObject ballEffect;
    public GameObject explosion;

    public override void Apply(SkillEffect effect)
    {
        ballEffect.SetActive(true);
        explosion.SetActive(false);

        sequence = DOTween.Sequence().
            Append(transform.DOMove(Turn.selectedPos, 1.0f).From(Turn.unit.pos))
                .OnComplete(() =>
                {
                    explosion.SetActive(true);
                    ballEffect.SetActive(false);
                    effect.Apply();
                });
    }

    public override float GetDuration()
    {
        return sequence.Duration() + 0.5f;
    }
}
