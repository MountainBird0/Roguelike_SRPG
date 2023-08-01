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

    public virtual void Enter()
    {
        Debug.Log($"{GetType()} - ����");
        if (board == null)
        {
            // ����ִ°� awake�� ������
            Debug.Log($"{GetType()} - ���� �ֱ�");
            board = BattleMapManager.instance.board;
        }
    }

    public virtual void Exit()
    {
        Debug.Log($"{GetType()} - ����");
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
