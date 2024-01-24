using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public Transform cameraPos;

    private Vector2 startPosition;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private Coroutine coroutine;

    public void Start()
    {
        SetMaxMinPos();

        InputManager.instance.OnStartTouch += DragStart;
        InputManager.instance.OnEndTouch += DragEnd;
    }

    public void OnDisable()
    {
        InputManager.instance.OnStartTouch -= DragStart;
        InputManager.instance.OnEndTouch -= DragEnd;
    }

    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    public void DragStart(Vector2 screenPosition, float time)
    {
        startPosition = InputManager.instance.PrimaryPosition();
        coroutine = StartCoroutine(CameraMove());
    }
    public void DragEnd(Vector2 screenPosition, float time)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    /**********************************************************
    * ī�޶� ������
    ***********************************************************/
    private IEnumerator CameraMove()
    {
        yield return new WaitForSeconds(0.1f);

        if (!InputManager.instance.isCameraLock)
        {
            while(true)
            {
                var directionForce = startPosition - InputManager.instance.PrimaryPosition();
                
                var interpolatedPos = new Vector3(cameraPos.position.x + directionForce.x, cameraPos.position.y + directionForce.y, -1);
                interpolatedPos.x = Mathf.Clamp(interpolatedPos.x, minX, maxX);
                interpolatedPos.y = Mathf.Clamp(interpolatedPos.y, minY, maxY);

                cameraPos.DOMove(interpolatedPos, 1.5f);
                yield return null;
            }
        }
        yield return null;
    }


    /**********************************************************
    * ���� �ʿ� ���� ī�޶� �ִ� �ּ� ���� ����
    ***********************************************************/
    private void SetMaxMinPos()
    {
        BoundsInt bounds = BattleMapManager.instance.board.mainMap.cellBounds;

        minX = bounds.xMin + 5.0f;
        maxX = bounds.xMax - 5.0f;
        minY = bounds.yMin; // + 7.0f;
        maxY = bounds.yMax; // - 7.0f;

        Debug.Log($"{GetType()} ���ð� - {minX}, {maxX}, {minY}, {maxY}");
    }
}
