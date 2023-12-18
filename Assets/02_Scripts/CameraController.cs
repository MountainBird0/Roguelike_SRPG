using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform cameraPos;

    private float minimumDistance = 0.2f;
    private float maximumTime = 1f;
    private float directionThreshold = 0.9f;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private Coroutine coroutine;

    public void Start()
    {
        Debug.Log($"{GetType()} - 이거 찍히나");
        InputManager.instance.OnStartTouch += DragStart;
        InputManager.instance.OnEndTouch += DragEnd;
    }

    public void OnDisable()
    {
        

        InputManager.instance.OnStartTouch -= DragStart;
        InputManager.instance.OnEndTouch -= DragEnd;
    }

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public void DragStart(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - 터치시작");
        //startPosition = screenPosition;
        startPosition = InputManager.instance.PrimaryPosition();
        startTime = time;

        coroutine = StartCoroutine(CameraMove());
    }
    public void DragEnd(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - 터치끝");
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        endPosition = screenPosition;
        endTime = time;
        DetectSwipe();
    }

    /**********************************************************
    * 카메라 움직임
    ***********************************************************/
    private IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(0.1f);

        if (InputManager.instance.clickResults.Count == 0)
        {
            while(true)
            {
                Debug.Log($"{GetType()} - 카메라 움직");
                var directionForce = startPosition - InputManager.instance.PrimaryPosition();

                Vector3 newPosition = new Vector3(directionForce.x, directionForce.y, -1);
                cameraPos.DOMove(newPosition, 1.5f).SetRelative();
                yield return null;
            }
        }
    }


    private void DetectSwipe()
    {
        Debug.Log($"{GetType()} - 계산");
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime - startTime) <= maximumTime)
        {
            Debug.Log($"{GetType()} - 그리기");
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);

            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log($"{GetType()} - up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log($"{GetType()} - down");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log($"{GetType()} - right");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log($"{GetType()} - left");
        }
    }

}
