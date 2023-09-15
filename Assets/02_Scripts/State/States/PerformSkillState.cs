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

        // ��ų ���� �� �� ����
    }

    public override void Exit()
    {
        base.Exit();
    }

    /**********************************************************
    * ��ų ã�Ƽ� ����
    ***********************************************************/
    private void DoSkill()
    {
        //var ob = Turn.unit.skills[Turn.slotNum];
        //var skill = Turn.unit.skills[Turn.slotNum].GetComponent<SkillEffect>();
        Turn.unit.skills[Turn.slotNum].GetComponent<SkillEffect>().Apply();
    }

}
