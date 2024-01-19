using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
    private TurnEndUIController uiController = BattleMapUIManager.instance.turnEndUIController;

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
            if(Turn.isHumanTurn == false)
            {
                uiController.StartEnemyTurn();
            }
        }
        else
        {
            if(BattleMapManager.instance.isAITurnFinish(Turn.unit.unitNum))
            {
                NewTurnStart();
            }
            else
            {
                if(BattleMapManager.instance.IsEnemyTurnStart())
                {
                    uiController.StartEnemyTurn();
                }                    
            }
        }

        Turn.Clear();
    }


    private void NewTurnStart()
    {
        Turn.turnCount++;
        if (Turn.turnCount == 16)
        {
            StateMachineController.instance.ChangeTo<StageDefeatState>();
            return;
        }
        uiController.StartNewTurn();
        BattleMapUIManager.instance.SetTurnCount();
        Turn.isHumanTurn = true;
        Debug.Log($"{GetType()} - {Turn.turnCount}번째 턴 시작");
    }
}
