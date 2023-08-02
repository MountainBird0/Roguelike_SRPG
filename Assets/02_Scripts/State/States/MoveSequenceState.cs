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
        // 움직이는 ani 추가
        
        StateMachineController.instance.ChangeTo<ChooseActionState>();

        //board.mainTiles[Turn.unit.tile.prev.pos].content.transform.position =
        //    Turn.unit.tile.pos;

        //board.mainTiles[Turn.unit.tile.prev.pos].content.transform.position =
        //    Turn.unit.tile.pos;

        //board.mainTiles[Turn.unit.tile.pos].content = board.mainTiles[Turn.unit.tile.prev.pos].content;
    }
}
