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
                    .AppendInterval(2.5f)
                    .OnComplete(() =>
                    {
                        ballEffect.SetActive(false);
                        // 점점 작아지게
                        // 다시 실행되도 그대로 실행되도록 수정

                        ObjectPoolManager.instance.Despawn(gameObject);
                    });

    }
}
