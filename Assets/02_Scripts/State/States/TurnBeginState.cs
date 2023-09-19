/**********************************************************
* 한 unit을 골라 그 unit의 turn을 시작하기 위한 State
***********************************************************/
using UnityEngine;

public class TurnBeginState : State
{
    private TurnBeginUIController uiController = BattleMapUIManager.instance.turnBeginUIController;

    public override void Enter()
    {
        base.Enter();

        if(Turn.unit)
        {
            SelectUnit(Turn.selectedTile.pos);
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;                   
        }

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
        

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

        uiController.DisableCanvas();

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
                var unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                if (unit.playerType.Equals(PlayerType.HUMAN))
                {
                    SelectUnit(cellPosition);
                }
                else
                {                   
                    uiController.ShowStatWindow(unit); // 정보창 보여주기
                }
            }
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 유닛 선택하기
    ***********************************************************/
    private void SelectUnit(Vector3Int cellPosition)
    {
        Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

        Turn.originTile = board.GetTile(cellPosition);
        Turn.currentTile = Turn.originTile;
        Turn.selectedTile = Turn.originTile;

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
