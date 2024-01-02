using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : SkillEffect
{
    public override void Apply()
    {
        Debug.Log($"{GetType()} - DamageEffect Apply");

        for (int i = 0; i < Turn.targets.Count; i++)
        {
            var target = Turn.targets[i];

            int damage = (int)(Turn.unit.stats.ATK * Turn.skill.data.multiplier);

            target.animationController.GotHit();
            target.SetHealth(-damage);
        }
    }
}


//int damage = Predict(target);
//int currentHP = target.GetStat(StatEnum.HP);
//float roll = Random.Range(1 - randomness, 1 + randomness);
//int finalDamage = Mathf.FloorToInt(damage * roll);

//target.SetStat(StatEnum.HP, -finalDamage);

//Debug.LogFormat("{0} estava com {1} de HP, foi afetado por {2}*{3}={4} e ficou com {5}",
//    target.name, currentHP, damage, roll, finalDamage, target.GetStat(StatEnum.HP));

////if(!isPrimary)
////return;

//if (target.GetStat(StatEnum.HP) <= 0)
//    target.animationController.Death(gotHitDelay);
//else
//{
//    target.animationController.Idle();
//    target.animationController.GotHit(gotHitDelay);
//}