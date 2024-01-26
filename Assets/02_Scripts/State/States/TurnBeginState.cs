/**********************************************************
* 한 unit을 골라 그 unit의 turn을 시작하기 위한 State
***********************************************************/
using UnityEngine;
using System.Linq;

public class TurnBeginState : State
{
    private TurnBeginUIController uiController = BattleMapUIManager.instance.turnBeginUIController;

    public override void Enter()
    {
        base.Enter();

        Turn.isMoving = true;

        // 이때 ui상태 확인

        if(BattleMapManager.instance.isBtnAuto)
        {
            Turn.isHumanTurn = false;
            BattleMapManager.instance.ChangeToAuto();
            SelectUnit(BattleMapManager.instance.AIUnits.First().Value.pos);
            return;
        }
        else
        {
            BattleMapManager.instance.ChangeToManual();
        }

        if (Turn.unit)
        {
            SelectUnit(Turn.selectedPos);
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;                   
        }
        // 움직일 수 있는 아군유닛 있는지 확인
        if(!Turn.isHumanTurn)
        {
            Debug.Log($"{GetType()} - AI턴");

            SelectUnit(BattleMapManager.instance.AIUnits.First().Value.pos);
            return;
        }

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;    
    }

    public override void Exit()
    {
        base.Exit();

        board.ClearTile();
        uiController.DisableCanvas();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (board.mainTiles.ContainsKey(cellPosition))
        {
            if (board.mainTiles[cellPosition].content)
            {
                var unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                if(BattleMapManager.instance.humanUnits.ContainsKey(unit.unitNum))
                {
                    SelectUnit(cellPosition);
                }
                else
                {                   
                    uiController.ShowStatWindow(unit); // 정보창 보여주기
                    ShowMoveableTile(unit);
                }
            }
        }
    }
    public override void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 유닛 선택하기
    ***********************************************************/
    private void SelectUnit(Vector3Int cellPosition)
    {
        Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

        Turn.originPos = cellPosition;
        Turn.selectedPos = Turn.originPos;

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    private void ShowMoveableTile(Unit unit)
    {
        board.ClearTile();
        var tiles = board.Search(board.GetTile(unit.pos), unit.ISMovable);
        board.ShowHighlightTile(tiles, 0);
    }
}
