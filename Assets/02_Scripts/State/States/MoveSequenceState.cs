/**********************************************************
* unit의 움직임이 이루어지는 State
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
    // 광역 힐등 본인포함 -> 이동한 위치 갱신 및 그 위치에서 타일서치 


    IEnumerator MoveSequence()
    {
        Turn.unit.tile = board.mainTiles[Turn.unit.pos];
        List<TileLogic> path = CreatePath();

        Movement movement = Turn.unit.GetComponent<Movement>();    
        yield return StartCoroutine(movement.Move(path));

        Turn.unit.SetPosition(Turn.selectedPos, board);
        yield return new WaitForSeconds(0.2f);

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private List<TileLogic> CreatePath()
    {
        board.Search(board.GetTile(Turn.unit.pos),100, IsMovable);

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

    private bool IsMovable(TileLogic from, TileLogic to, int range)
    {
        to.distance = from.distance + 1;

        return (to.content == null && to.distance <= range && highlightTiles.ContainsKey(to.pos));
    }


}
