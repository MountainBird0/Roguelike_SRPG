using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    public override void Enter()
    {
        base.Enter();

        board.ClearTile();
        ClearCheck();
        ActableUnitCheck();

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void ClearCheck()
    {
        BattleMapManager.instance.ClearCheck();
    }

    private void ActableUnitCheck()
    {
        if(Turn.isHumanTurn)
        {
            Turn.isHumanTurn = BattleMapManager.instance.isHumanTurnFinish(Turn.unit.unitNum);
        }
        else
        {
            if(BattleMapManager.instance.isAITurnFinish(Turn.unit.unitNum))
            {
                // 턴 오르는 ui // 다음 턴 시작
                Turn.turnCount++;
                Turn.isHumanTurn = true;
                Debug.Log($"{GetType()} - {Turn.turnCount}번째 턴 시작");
            }
        }
        Turn.Clear();
    }
}
