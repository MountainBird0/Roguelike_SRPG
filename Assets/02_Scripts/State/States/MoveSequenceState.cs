/**********************************************************
* unit의 움직임이 이루어지는 State
***********************************************************/
using System;
using System.Collections;
using UnityEngine;

public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();

        if (!Turn.isHumanTurn)
        {
            StartCoroutine(AIChooseAction());
        }
        else
        {
            MoveUnit();
        }
    }

    public override void Exit()
    {
        base.Exit();

    }
    // 광역 힐같은거 할때 자기자신도 할려면 이동할때마다 스킬범위 검색할때 검색되도록 값 넣기 
    private void MoveUnit()
    {
        Debug.Log($"{GetType()} - 이동 전{Turn.unit.gameObject.transform.position}");
        Turn.unit.gameObject.transform.position = Turn.selectedTile.pos;
        Debug.Log($"{GetType()} - 이동 후{Turn.unit.gameObject.transform.position}");


        Turn.hasMoved = true;
        // 움직이는 ani 추가

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private IEnumerator AIChooseAction()
    {
        Debug.Log($"{GetType()} - 유닛{Turn.unit.name}");

        Debug.Log($"{GetType()} - 2{aiController.aiPlan.movePos}");
        Debug.Log($"{GetType()} - 1{Turn.unit.gameObject.transform.position}");
        Turn.unit.gameObject.transform.position = aiController.aiPlan.movePos;
        Debug.Log($"{GetType()} - 이동 후{aiController.aiPlan.movePos}");
        Debug.Log($"{GetType()} - 이동 후{Turn.unit.gameObject.transform.position}");
        Turn.unit.currentPos = aiController.aiPlan.movePos;
        Turn.hasMoved = true;

        yield return new WaitForSeconds(1f);
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
