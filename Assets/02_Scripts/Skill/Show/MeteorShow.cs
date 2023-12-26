using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteorShow : MonoBehaviour
{
    public GameObject ballEffect;
    public GameObject groundEffect;

    private Sequence sequence;

    private void Start()
    {
        sequence = DOTween.Sequence()
                    .OnStart(() =>
                    {
                        ballEffect.transform.DOLocalMove(new Vector3(-5, 5, 5), 1.5f).From(true)
                        .OnComplete(() => groundEffect.SetActive(true));
                    })
                    .AppendInterval(1.7f)
                    .OnComplete(() =>
                    {
                        ballEffect.SetActive(false);
                        ObjectPoolManager.instance.Despawn(gameObject);
                    });

    }
}
