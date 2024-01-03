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
    * 스킬 찾아서 실행
    ***********************************************************/
    private IEnumerator ApplySkill()
    {
        // 나중에 CAS에서 스킬 오브젝트 넣는것으로 변경
        // 간소화 ui추가

        // selectedPos ai할땐 없는듯
        var quat = Turn.unit.tile.GetDirection(Turn.selectedPos);
        if (quat.HasValue)
        {
            Turn.unit.body.transform.rotation = quat.Value;
        }

        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for (int i = 0; i < effects.Count; i++)
        {
            Debug.Log($"{GetType()} - 스킬이름 : {effects[i].effectName}");
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
    * 스킬 쿨타임 세팅
    ***********************************************************/
    private void SetCoolTime()
    {
        int defaultCoolTime = Turn.skill.data.coolTime;

        for (int i = 0; i < Turn.unit.skills.Count; i++)
        {
            var skill = Turn.unit.skills[i].GetComponent<Skill>();

            if (Turn.skill.data.name == skill.data.name)
            {
                if (defaultCoolTime != 0) // 쿨타임이 0인 스킬은 넘어감
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
