using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkillState : State
{
    public override void Enter()
    {
        base.Enter();


        SetCoolTime();
        StartCoroutine(ApplySkill());
        //DoSkill();
    }

    public override void Exit()
    {
        base.Exit();
    }

    /**********************************************************
    * ��ų ã�Ƽ� ����
    ***********************************************************/
    private IEnumerator ApplySkill()
    {
        // ���߿� CAS���� ��ų ������Ʈ �ִ°����� ����
        // ����ȭ ui�߰�

        // selectedPos ai�Ҷ� ���µ�
        var quat = Turn.unit.tile.GetDirection(Turn.selectedPos);
        if (quat.HasValue)
        {
            Turn.unit.body.transform.rotation = quat.Value;
        }

        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for (int i = 0; i < effects.Count; i++)
        {
            Debug.Log($"{GetType()} - ��ų�̸� : {effects[i].effectName}");
            GameObject ob = ObjectPoolManager.instance.Spawn(effects[i].effectName);
            if(ob != null)
            {
                var visualEffect = ob.GetComponent<SkillVisualEffect>();
                visualEffect.Apply(effects[i]);

                yield return new WaitForSeconds(visualEffect.GetDuration());

                ObjectPoolManager.instance.Despawn(ob);
            }
        }
        StateMachineController.instance.ChangeTo<TurnEndState>();
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

}
