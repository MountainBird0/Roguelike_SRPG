using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TurnEndUIController : MonoBehaviour
{
    public Canvas turnEndCanvas;

    [Header("BackGround")]
    public GameObject backTop;
    public GameObject backBottom;

    [Header("Enemy")]
    public GameObject enemyTurn;

    [Header("NewTurn")]
    public GameObject newTurn;
    public TextMeshProUGUI turnCount;

    public void StartEnemyTurn()
    {
        turnEndCanvas.gameObject.SetActive(true);
        BackGroundAni(enemyTurn);     
    }

    public void StartNewTurn()
    {
        turnEndCanvas.gameObject.SetActive(true);
        turnCount.alpha = 1f;
        turnCount.text = Turn.turnCount.ToString() + " / 15";
        BackGroundAni(newTurn);
    }


    private void BackGroundAni(GameObject ob)
    {
        var rt1 = backTop.GetComponent<RectTransform>();
        var rt2 = backBottom.GetComponent<RectTransform>();
        
        ob.SetActive(true);

        Sequence sequence = DOTween.Sequence()
            .OnStart(() =>
                {
                    ob.GetComponent<TextMeshProUGUI>().alpha = 1f;
                    rt1.transform.localPosition = new Vector3(-1100, 360, 0);
                    rt2.transform.localPosition = new Vector3(1100, -360, 0);
                })  
            .Append(rt1.DOAnchorPosX(800, 1.3f).SetRelative())
            .Join(rt2.DOAnchorPosX(-800, 1.3f).SetRelative())
                .OnComplete(() =>
                    {
                        ob.GetComponent<TextMeshProUGUI>().DOFade(0f, 0.3f);
                        turnCount.DOFade(0f, 0.3f);
                        rt1.DOAnchorPosX(1600, 1.7f).SetRelative();
                        rt2.DOAnchorPosX(-1600, 1.7f).SetRelative();
                    });
    }



}
