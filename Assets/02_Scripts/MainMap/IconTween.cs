/**********************************************************
* æ∆¿Ãƒ‹ tweening 
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IconTween : MonoBehaviour
{
    public float scaleDuration = 1f;
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    public void LiveIcon()
    {
        transform.DOScale(maxScale, scaleDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
    }  
}
