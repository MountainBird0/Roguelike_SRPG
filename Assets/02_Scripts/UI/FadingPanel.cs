using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadingPanel : MonoBehaviour
{
	[SerializeField] private CanvasGroup canvasGroup;
	private Tween fadeTween;

	void Start()
	{
		StartCoroutine(TestFade());
	}

	public void FadeIn(float duration)
	{
		Fade(1.0f, duration);
	}

	public void FadeOut(float duration)
	{
		Fade(0.0f, duration);
	}

	private void Fade(float endValue, float duration)
	{
		if (fadeTween != null)
		{
			fadeTween.Kill(false);
		}
		fadeTween = canvasGroup.DOFade(endValue, duration);
	}

	private IEnumerator TestFade()
	{
		yield return new WaitForSeconds(2f);
		FadeOut(1f);
		yield return new WaitForSeconds(3f);
		FadeIn(1f);
	}
}
