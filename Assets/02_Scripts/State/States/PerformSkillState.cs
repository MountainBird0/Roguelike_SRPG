using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkillState : State
{
    public override void Enter()
    {
        base.Enter();

        DoSkill();

        StateMachineController.instance.ChangeTo<TurnEndState>();

        // 스킬 실행 후 턴 종료
    }

    public override void Exit()
    {
        base.Exit();
    }

    /**********************************************************
    * 스킬 찾아서 실행
    ***********************************************************/
    private void DoSkill()
    {
        //var ob = Turn.unit.skills[Turn.slotNum];
        //var skill = Turn.unit.skills[Turn.slotNum].GetComponent<SkillEffect>();
        Turn.unit.skills[Turn.slotNum].GetComponent<SkillEffect>().Apply();
    }

}
