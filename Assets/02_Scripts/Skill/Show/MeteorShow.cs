using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorShow : SkillVisualEffect
{
    public GameObject ballEffect;
    public GameObject groundEffect;

    public override void Apply(SkillEffect effect)
    {
        groundEffect.SetActive(false);

        sequence = DOTween.Sequence()
            .Append(ballEffect.transform.DOMove(Turn.selectedPos, 1.5f).From(new Vector3(-5, 5, 5))
                .OnComplete(() =>
                {
                    effect.Apply();
                    groundEffect.transform.position = new Vector3Int(Turn.selectedPos.x, Turn.selectedPos.y, 5);
                    groundEffect.SetActive(true);
                }))
            .Append(ballEffect.transform.DOScale(Vector3.zero, 1.0f).From(Vector3.one));
    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }
}
