using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEndState : State
{
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
        if(Turn.currentTile.pos != Turn.originTile.pos)
        {
            board.mainTiles[Turn.currentTile.pos].content = board.mainTiles[Turn.originTile.pos].content;
            board.mainTiles[Turn.originTile.pos].content = null;
        }
    }

    private void SetCoolTime()
    {
        int defaultCoolTime = Turn.currentSkill.coolTime;

        for (int i = 0; i < 3; i++)
        {
            var skill = Turn.unit.skills[i].GetComponent<Skill>();

            if (Turn.slotNum.Equals(i))
            {
                if (defaultCoolTime != 0) // 쿨타임이 0인 스킬은 넘어감
                {
                    skill.SetCoolTime(defaultCoolTime);
                }
            }
            else if (skill.coolTime > 0)
            {
                skill.ReduceCoolTime();
            }
        }
    }

    private void TurnClear()
    {
        // Turn.unit.isTurnEnd = true;
        Turn.slotNum = -1;
        Turn.currentSkill = null;
        
        Turn.unit = null;
    }



}
