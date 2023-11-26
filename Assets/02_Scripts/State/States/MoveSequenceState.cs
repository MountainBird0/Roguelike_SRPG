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
            return;
        }

         MoveUnit();      
    }

    public override void Exit()
    {
        base.Exit();

    }
    // 광역 힐같은거 할때 자기자신도 하려면 이동할때마다 스킬범위 검색할때 검색되도록 값 넣기 
    private void MoveUnit()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // 움직이는 ani 추가

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private IEnumerator AIChooseAction()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // 움직이는 ani 추가
        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();

        //Debug.Log($"{GetType()} - 움직일 유닛 : {Turn.unit.name}");
        //Debug.Log($"{GetType()} - 움직일 곳 : {aiController.aiPlan.movePos}");

        //Turn.unit.gameObject.transform.position = aiController.aiPlan.movePos;

        //Turn.unit.pos = aiController.aiPlan.movePos;
        //Turn.hasMoved = true;

        //board.mainTiles[Turn.currentTile.pos].content = board.mainTiles[Turn.originTile.pos].content;
        //board.mainTiles[Turn.originTile.pos].content = null;

        //yield return new WaitForSeconds(1f);
        //StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
