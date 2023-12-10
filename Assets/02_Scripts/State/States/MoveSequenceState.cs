/**********************************************************
* unit�� �������� �̷������ State
***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();

        StartCoroutine(MoveSequence());

        //if (!Turn.isHumanTurn)
        //{
        //    StartCoroutine(AIChooseAction());
        //    return;
        //}

        // MoveUnit();      
    }

    public override void Exit()
    {
        base.Exit();

    }
    // ���� �������� �Ҷ� �ڱ��ڽŵ� �Ϸ��� �̵��Ҷ����� ��ų���� �˻��Ҷ� �˻��ǵ��� �� �ֱ� 
    private void MoveUnit()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // �����̴� ani �߰�

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private IEnumerator AIChooseAction()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedPos;

        // �����̴� ani �߰�
        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    IEnumerator MoveSequence()
    {
        Turn.isMoving = true;
        Turn.unit.tile = board.mainTiles[Turn.unit.pos];
        List<TileLogic> path = CreatePath();
        Movement movement = Turn.unit.GetComponent<Movement>();
        yield return StartCoroutine(movement.Move(path));

        board.mainTiles[Turn.selectedPos].content = board.mainTiles[Turn.unit.pos].content;
        board.mainTiles[Turn.unit.pos].content = null;

        Turn.currentPos = Turn.selectedPos;
        Turn.unit.pos = Turn.currentPos;
        Turn.isMoving = false;
        yield return new WaitForSeconds(0.2f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private List<TileLogic> CreatePath()
    {
        List<TileLogic> path = new();

        TileLogic targetTile = board.mainTiles[Turn.selectedPos];
        while(targetTile != Turn.unit.tile)
        {
            path.Add(targetTile);
            targetTile = targetTile.prev;
        }

        path.Reverse();
        return path;
    }
}
