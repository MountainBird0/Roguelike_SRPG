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
    private void DoSkill()
    {
        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for(int i = 0; i < effects.Count; i++)
        {
            effects[i].Apply();
        }

        // effect ť�� �־ �� ������ �Ѿ��
        // StateMachineController.instance.ChangeTo<TurnEndState>(); 
    }

    private IEnumerator ApplySkill()
    {
        // ���߿� CAS���� ��ų ������Ʈ �ִ°����� ����
        // ����ȭ ui�߰�

        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for (int i = 0; i < effects.Count; i++)
        {
            Debug.Log($"{GetType()} - ��ų�̸� : {Turn.skill.image.name}");
            GameObject ob = ObjectPoolManager.instance.Spawn(Turn.skill.image.name);
            ob.transform.position = new Vector3Int(Turn.selectedPos.x, Turn.selectedPos.y, 3);

            yield return new WaitForSeconds(effects[i].delay);
            
            effects[i].Apply();
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
