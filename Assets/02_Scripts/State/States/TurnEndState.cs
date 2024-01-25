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
        SetCoolTime();

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
    * ��ų ��Ÿ�� ����
    ***********************************************************/
    private void SetCoolTime()
    {
        
        int defaultCoolTime = 0;
        string skillName = null;
        
        if(Turn.skill != null)
        {
            defaultCoolTime = Turn.skill.data.coolTime;
            skillName = Turn.skill.data.name;
        }

        for (int i = 0; i < Turn.unit.skills.Count; i++)
        {
            var skill = Turn.unit.skills[i].GetComponent<Skill>();

            if (skillName == skill.data.name)
            {
                if (defaultCoolTime != 0) // ��Ÿ���� 0�� ��ų�� �Ѿ
                {
                    skill.SetCoolTime(defaultCoolTime);
                }
            }
            else if (skill.data.currentCoolTime > 0)
            {
                skill.ReduceCoolTime();
            }
        }
    }


    /**********************************************************
    * �̹��Ͽ� Ȱ�� ������ ������ �ִ��� Ȯ��
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

        Turn.unit.tile = board.GetTile(Turn.unit.pos);
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
        Debug.Log($"{GetType()} - {Turn.turnCount}��° �� ����");
    }
}
