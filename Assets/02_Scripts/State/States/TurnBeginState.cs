using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBeginState : State
{
    private Dictionary<Vector3Int, TileLogic> mainTiles;



    public override void Enter()
    {
        base.Enter();

        mainTiles = BattleMapManager.instance.mainTiles;

        // 움직일 아군 유닛이 없으면 적 자동 이동으로
        //if()

        Turn.unit = BattleMapManager.instance.units[0];

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
        if(mainTiles.ContainsKey(cellPosition))
        {
            if (mainTiles[cellPosition].content)
            {
                Turn.unit = mainTiles[cellPosition].content.GetComponent<Unit>();
                StateMachineController.instance.ChangeTo<ChooseActionState>();
            }
        }


    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    


}
