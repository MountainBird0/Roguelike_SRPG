using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Scrolling : MonoBehaviour
{
    public TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Scroll(Color color, int value)
    {
        text.color = color;
        text.text = value.ToString();

        transform.DOMoveY(1.5f, 1f).SetRelative()
            .OnComplete(() => ObjectPoolManager.instance.Despawn(this.gameObject));
    }
}
