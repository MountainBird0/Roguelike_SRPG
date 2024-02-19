using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trailer : MonoBehaviour
{
    public GameObject bubbleTrail;

    private void Start()
    {
        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    private void OnDisable()
    {

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - ´©¸§");
        var mousePos = InputManager.instance.PrimaryPosition();
        
        bubbleTrail.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        bubbleTrail.SetActive(true);
        bubbleTrail.transform.DOMoveZ(3, 1f).OnComplete(() => bubbleTrail.SetActive(false));



    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        //var mousePos = InputManager.instance.PrimaryPosition();
        //bubbleTrail.SetActive(false);
        //bubbleTrail.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
}
