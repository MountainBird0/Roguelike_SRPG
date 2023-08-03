using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBeginState : State
{
    public override void Enter()
    {
        base.Enter();
    
        if(Turn.unit)
        {
            // ���� turn�� unit�� �ִٸ� 
            StateMachineController.instance.ChangeTo<ChooseActionState>();
        }

        // �Ʊ� ���� ������ �� ���� �ӵ���� ����
        // �ӵ� ���� ���� ����
        //if(!BattleMapManager.instance.IsHuman())
        //{
        //    Turn.unit = BattleMapManager.instance.units[0];
        //    StateMachineController.instance.ChangeTo<ChooseActionState>();
        //}
   
        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;     
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if(board.mainTiles.ContainsKey(cellPosition))
        {
            if (board.mainTiles[cellPosition].content)
            {
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                Turn.prevTile = board.GetTile(cellPosition);
                StateMachineController.instance.ChangeTo<ChooseActionState>();
            }
        }
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * Turn.unit.tile.pos
    * mainTiles.content
    * mainTiles.content.transform.position
    * Unit�� �ִ� dic�����
    ***********************************************************/

}
