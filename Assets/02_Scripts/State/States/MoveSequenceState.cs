/**********************************************************
* unit�� �������� �̷������ State
***********************************************************/
using System;
using System.Collections;
using UnityEngine;

public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();

        if (!Turn.isHumanTurn)
        {
            StartCoroutine(AIChooseAction());
            return;
        }

         MoveUnit();      
    }

    public override void Exit()
    {
        base.Exit();

    }
    // ���� �������� �Ҷ� �ڱ��ڽŵ� �Ϸ��� �̵��Ҷ����� ��ų���� �˻��Ҷ� �˻��ǵ��� �� �ֱ� 
    private void MoveUnit()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedTile.pos;
        Turn.unit.pos = Turn.selectedTile.pos;

        Turn.hasMoved = true;
        // �����̴� ani �߰�

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private IEnumerator AIChooseAction()
    {
        Debug.Log($"{GetType()} - ����{Turn.unit.name}");

        Debug.Log($"{GetType()} - 2{aiController.aiPlan.movePos}");
        Debug.Log($"{GetType()} - 1{Turn.unit.gameObject.transform.position}");

        Turn.unit.gameObject.transform.position = aiController.aiPlan.movePos;

        Debug.Log($"{GetType()} - �̵� ��{aiController.aiPlan.movePos}");
        Debug.Log($"{GetType()} - �̵� ��{Turn.unit.gameObject.transform.position}");

        Turn.unit.pos = aiController.aiPlan.movePos;
        Turn.hasMoved = true;

        yield return new WaitForSeconds(1f);
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
