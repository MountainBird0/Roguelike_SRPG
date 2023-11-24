using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkillState : State
{
    public override void Enter()
    {
        base.Enter();

        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AIPerformSkill());
        }


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
        Turn.unit.skills[Turn.skillSlotNum].GetComponent<SkillEffect>().Apply();
    }

    private IEnumerator AIPerformSkill()
    {
        // ���߿� CAS���� ��ų ������Ʈ �ִ°����� ����
        // ����ȭ ui�߰�
        DoSkill();

        yield return new WaitForSeconds(1f);
        StateMachineController.instance.ChangeTo<TurnEndState>();
    }

}
