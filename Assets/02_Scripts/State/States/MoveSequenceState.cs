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
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // �����̴� ani �߰�

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private IEnumerator AIChooseAction()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // �����̴� ani �߰�
        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
