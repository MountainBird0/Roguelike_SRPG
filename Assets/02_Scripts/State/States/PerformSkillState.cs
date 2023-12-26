using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkillState : State
{
    public override void Enter()
    {
        base.Enter();

        StartCoroutine(PerformSkill());
        return;
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
        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for(int i = 0; i < effects.Count; i++)
        {
            effects[i].Apply();
        }
    }

    /**********************************************************
    * ��ų ��Ÿ�� ����
    ***********************************************************/
    private void SetCoolTime()
    {
        int defaultCoolTime = Turn.skill.data.coolTime;

        for (int i = 0; i < Turn.unit.skills.Count; i++)
        {
            var skill = Turn.unit.skills[i].GetComponent<Skill>();

            if (Turn.skill.data.name == skill.data.name)
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

    private IEnumerator PerformSkill()
    {
        // ���߿� CAS���� ��ų ������Ʈ �ִ°����� ����
        // ����ȭ ui�߰�
        DoSkill();
        
        yield return new WaitForSeconds(3f);

        SetCoolTime();

        StateMachineController.instance.ChangeTo<TurnEndState>();
    }
}
