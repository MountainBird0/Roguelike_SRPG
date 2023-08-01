using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBeginState : State
{
    private Dictionary<Vector3Int, TileLogic> mainTiles;

    public override void Enter()
    {
        base.Enter();

        // 현재 turn unit이 있는지 확인
        if(Turn.unit != null)
        {
            StateMachineController.instance.ChangeTo<ChooseActionState>();
        }

        // 아군 유닛 없으면 적 유닛 속도대로 정렬
        // 속도 정렬 아직 안함
        //if(!BattleMapManager.instance.IsHuman())
        //{
        //    Turn.unit = BattleMapManager.instance.units[0];
        //    StateMachineController.instance.ChangeTo<ChooseActionState>();
        //}

        mainTiles = BattleMapManager.instance.mainTiles; // 이거 그때그때 안하고 첨 생길 때 가지고있기
   
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

    /**********************************************************
    * 
    ***********************************************************/

}
