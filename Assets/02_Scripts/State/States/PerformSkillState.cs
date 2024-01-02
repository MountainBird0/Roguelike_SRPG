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
    private void DoSkill()
    {
        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for(int i = 0; i < effects.Count; i++)
        {
            effects[i].Apply();
        }

        // effect 큐에 넣어서 다 끝나면 넘어갈까
        // StateMachineController.instance.ChangeTo<TurnEndState>(); 
    }

    private IEnumerator ApplySkill()
    {
        // 나중에 CAS에서 스킬 오브젝트 넣는것으로 변경
        // 간소화 ui추가

        var effects = Turn.skill.effects;

        Turn.unit.animationController.PhysicsAttack();

        for (int i = 0; i < effects.Count; i++)
        {
            // 이펙트에 맞는 시각효과 찾아 생성
            // 시각효과 끝날때까지 대기
            // 해당 이펙트 발동
            // 다음으로 넘어감


            Debug.Log($"{GetType()} - 스킬이름 : {effects[i].effectName}");
            GameObject ob = ObjectPoolManager.instance.Spawn(effects[i].effectName);
            if(ob != null)
            {
                Debug.Log($"{GetType()} - 땡겨오기 성공");
                var visualEffect = ob.GetComponent<SkillVisualEffect>();
                visualEffect.Apply(effects[i]);

                yield return new WaitForSeconds(visualEffect.GetDuration());
            }

           
            // yield return new WaitForSeconds(effects[i].delay);
            
            // effects[i].Apply();
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
