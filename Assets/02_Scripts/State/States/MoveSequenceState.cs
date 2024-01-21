/**********************************************************
* unit�� �������� �̷������ State
***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : State
{
    public Dictionary<Vector3Int, TileLogic> highlightTiles = new();

    public override void Enter()
    {
        base.Enter();

        Turn.isMoving = false;

        highlightTiles = board.highlightTiles;

        StartCoroutine(MoveSequence());    
    }

    public override void Exit()
    {
        base.Exit();

    }
    // ���� ���� �������� -> �̵��� ��ġ ���� �� �� ��ġ���� Ÿ�ϼ�ġ 


    IEnumerator MoveSequence()
    {
        Turn.unit.tile = board.mainTiles[Turn.unit.pos];
        List<TileLogic> path = CreatePath();

        Movement movement = Turn.unit.GetComponent<Movement>();
        yield return StartCoroutine(movement.Move(path));

        if (path.Count == 0)
        {
            Turn.unit.SetPosition(Turn.selectedPos, board);

        }
        else
        {
            Turn.unit.SetPosition(path[^1].pos, board);
        }

        yield return new WaitForSeconds(0.2f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private List<TileLogic> CreatePath()
    {
        List<TileLogic> path = new();
        TileLogic targetTile = board.mainTiles[Turn.selectedPos];
        board.Search(board.GetTile(Turn.unit.pos), IsMovable);

        //if (!Turn.isHumanTurn)
        //{
        //    while(targetTile != Turn.unit.tile)
        //    {
        //        Debug.Log($"{GetType()} - Ÿ�� ��ġ {targetTile.pos}");
        //        Debug.Log($"{GetType()} - Ÿ�� �Ÿ� {targetTile.distance}");
        //        if (targetTile.distance <= Turn.unit.stats.MOV)
        //        {
        //            Debug.Log($"{GetType()} - Ÿ�� ����");
        //            path.Add(targetTile);
        //        }
        //        targetTile = targetTile.prev;
        //    }
        //}


        while(targetTile != Turn.unit.tile)
        {
            path.Add(targetTile);
            targetTile = targetTile.prev;
        }
        path.Reverse();
        return path;
    }

    private bool IsMovable(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;

        //return ((to.content == null || to.pos == Turn.selectedPos)
        //    && to.distance <= 10);
        return (to.content == null && to.distance <= 30 && highlightTiles.ContainsKey(to.pos));
    }


}
