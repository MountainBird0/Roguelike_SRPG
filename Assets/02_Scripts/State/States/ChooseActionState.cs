using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionState : State
{
    // 움직일수있는 범위 표시
    // 움직이기
    // 움직인 위치에서 
    // 움직임스킬 사용 상태
    // 턴 넘기기 버튼

    private Dictionary<Vector3Int, TileLogic> mainTiles;

    // test
    int range = 3;
    List<TileLogic> tiles;


    public override void Enter()
    {
        base.Enter();
        mainTiles = BattleMapManager.instance.mainTiles;

        

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
        if (mainTiles.ContainsKey(cellPosition))
        {
            if (mainTiles[cellPosition].content != null) // 내 유닛일때만
            {
                tiles = BattleMapManager.instance.Search(BattleMapManager.instance.GetTile(cellPosition), ValidateMovement);

                foreach (var pair in tiles)
                {
                    Debug.Log($"{GetType()} main Key: " + pair + ", Value: " + pair.pos);
                }

                //StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    // test
    public virtual bool ValidateMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        //to.distance = from.distance+from.moveCost;

        if (to.content != null || to.distance > range)
        {
            return false;
        }
        return true;
    }
}
