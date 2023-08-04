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
        Turn.unit.gameObject.transform.position = Turn.selectedTile.pos;
        Turn.hasMoved = true;
        // 움직이는 ani 추가

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
