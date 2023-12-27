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
                        // ���� �۾�����
                        // �ٽ� ����ǵ� �״�� ����ǵ��� ����

                        ObjectPoolManager.instance.Despawn(gameObject);
                    });

    }
}
