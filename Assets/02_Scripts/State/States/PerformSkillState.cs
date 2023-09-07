using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkillState : State
{
    public override void Enter()
    {
        base.Enter();

        DoSkill();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;

        // ��ų ���� �� �� ����
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
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



    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {

    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }
}
