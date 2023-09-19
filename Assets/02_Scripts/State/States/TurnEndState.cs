using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    private ChooseActionUIController uiController = BattleMapUIManager.instance.ChooseActionUIController;

    public override void Enter()
    {
        base.Enter();

        SetUnitPos();
        SetCoolTime();
        TurnClear();
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SetUnitPos()
    {
        board.mainTiles[Turn.currentTile.pos].content = board.mainTiles[Turn.originTile.pos].content;
        board.mainTiles[Turn.originTile.pos].content = null;
        // Turn.currentTile


    }


    private void SetCoolTime()
    {
        uiController.SetCoolTime(Turn.slotNum);
    }

    private void TurnClear()
    {
        Turn.unit = null;
    }



}
