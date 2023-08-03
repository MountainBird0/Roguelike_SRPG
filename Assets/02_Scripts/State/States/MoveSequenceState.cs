using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();
        MoveUnit();

    }

    public override void Exit()
    {
        base.Exit();

    }

    private void MoveUnit()
    {
        Turn.hasMoved = true;
        Turn.unit.gameObject.transform.position = Turn.selectedTile.pos;
        // �����̴� ani �߰�

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
