using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : SkillEffect
{
    public override void Apply()
    {
        Debug.Log($"{GetType()} - HealEffect Apply");
        for(int i = 0; i < Turn.targets.Count; i++)
        {
            Debug.Log($"{GetType()} - Èú Àü {Turn.targets[i].stats.HP}");

            int heal = (int)(Turn.unit.stats.ATK * Turn.currentSkill.multiplier);

            Turn.targets[i].stats.HP += heal;

            if(Turn.targets[i].stats.MaxHP < Turn.targets[i].stats.HP)
            {
                Turn.targets[i].stats.HP = Turn.targets[i].stats.MaxHP;
            }

            Turn.targets[i].SetHealthBar();

            Debug.Log($"{GetType()} - Èú ÈÄ {Turn.targets[i].stats.HP}");
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