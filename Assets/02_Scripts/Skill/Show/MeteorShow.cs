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
        ballEffect.SetActive(true);
        ballEffect.transform.localScale = Vector3.one;
        groundEffect.SetActive(false);

        transform.position = new Vector3Int(Turn.selectedPos.x, Turn.selectedPos.y, 3);

        sequence = DOTween.Sequence()
            .OnStart(() =>
            {
                ballEffect.transform.DOMove(Turn.selectedPos, 1.5f).From(new Vector3(-5, 5, 5))
                .OnComplete(() =>
                {
                    groundEffect.SetActive(true);
                    effect.Apply();
                    ballEffect.transform.DOScale(Vector3.zero, 1.0f).From(Vector3.one);
                });
            })
            .AppendInterval(2.3f)
            .OnComplete(() =>
            {
                ballEffect.SetActive(false);

            // ���� �۾�����
            // �ٽ� ����ǵ� �״�� ����ǵ��� ����
            // ���⼭ ������ ���°� ����

            ObjectPoolManager.instance.Despawn(gameObject);
            });

        Debug.Log($"{GetType()} - ���� ����? {sequence.Duration()}");
    }

    public override float GetDuration()
    {
        return sequence.Duration();
    }
}
