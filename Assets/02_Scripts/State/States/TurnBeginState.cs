/**********************************************************
* 한 unit을 골라 그 unit의 turn을 시작하기 위한 State
***********************************************************/
using UnityEngine;

public class TurnBeginState : State
{
    public override void Enter()
    {
        base.Enter();

        if(Turn.unit)
        {
            StateMachineController.instance.ChangeTo<ChooseActionState>();       
        }
        else
        {
            InputManager.instance.OnStartTouch += TouchStart;
            InputManager.instance.OnEndTouch += TouchEnd;
        }

        // 아군 유닛 없으면 적 유닛 속도대로 정렬
        // 속도 정렬 아직 안함
        //if(!BattleMapManager.instance.IsHuman())
        //{
        //    Turn.unit = BattleMapManager.instance.units[0];
        //    StateMachineController.instance.ChangeTo<ChooseActionState>();
        //}
        
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (board.mainTiles.ContainsKey(cellPosition))
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
    * Unit만 있는 dic만들까
    ***********************************************************/

}
