using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    public override void Enter()
    {
        base.Enter();

        board.ClearTile();

        var isClear = BattleMapManager.instance.ClearCheck();
        if (isClear.HasValue)
        {
            if(isClear.Value)
            {
                StateMachineController.instance.ChangeTo<StageClearState>();
            }
            else
            {
                StateMachineController.instance.ChangeTo<StageDefeatState>();

            }
            return;
        }


        ActableUnitCheck();
        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    public override void Exit()
    {
        base.Exit();
    }

    /**********************************************************
    * 이번턴에 활동 가능한 유닛이 있는지 확인
    ***********************************************************/
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
