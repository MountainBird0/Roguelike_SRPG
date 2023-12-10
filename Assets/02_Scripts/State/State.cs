using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // ���� ������ ������ ui, index��
    // input�� ����
    // board�� ����
    protected Board board;

    //protected StateMachineController machine { get { return StateMachineController.instance; } };

    public void OnDisable()
    {
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    public virtual void Enter()
    {
        Debug.Log($"{GetType()} - ����");
        if (board == null)
        {          
            board = BattleMapManager.instance.board; // ����ִ°� awake�� ������
        }
    }

    public virtual void Exit()
    {
        Debug.Log($"{GetType()} - ����");
    }

    public virtual void TouchStart(Vector2 screenPosition, float time)
    {

    }
    public virtual void TouchEnd(Vector2 screenPosition, float time)
    {

    }


    // Ÿ�� �̵�, ui ���� �� �Լ���

    /**********************************************************
    * ���콺 ��ġ Ÿ�Ͽ� �°� ��ȯ
    ***********************************************************/
    protected Vector3Int GetCellPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;

        return board.deployMap.WorldToCell(worldPosition);
    }

}
