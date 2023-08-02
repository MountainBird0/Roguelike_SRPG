using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionState : State
{
    private ChooseActionUIController uiController = BattleMapUIManager.instance.ActionUI;
    
    // 움직일수있는 범위 표시
    // 움직이기
    // 움직인 위치에서 
    // 움직임스킬 사용 상태
    // 턴 넘기기 버튼

    // test
    private int range = 3;

    private List<TileLogic> tiles;

    public override void Enter()
    {
        base.Enter();

        uiController.EnableCanvas();

        if(!Turn.hasMoved)
        {
            ShowMoveableTile();
        }

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();

        uiController.DisableCanvas();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        // 1. 누른곳이 아무곳도 아닐때
        // -> 그냥 TurnBeginState로 

        // 2. 누른곳이 다른 unit 일 때
        // -> Turn.unit 그걸로 바꾸고 TurnBeginState로

        // 3. 누른곳이 자기자신 일 때
        // -> 세부정보 표시??

        // 4. 누른곳이 움직일 수 있는 장소라면
        // -> MoveSequenceState로

        // 5. 스킬 아이콘 누르면
        // -> 실제 unit의 position(selectedTile)로 content의 unit 변경해야할듯?

        // 이동가능한 타일을 터치했다면
        if (board.highlightTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
        }
        // 이동 불가능한 곳을 터치했다면
        else if (board.mainTiles.ContainsKey(cellPosition))
        {      
            // 누른곳에 이미 유닛이 있다면
            if (board.mainTiles[cellPosition].content) // 내 유닛일때만 추가
            {
                Debug.Log($"{GetType()} - 이 유닛으로 변경");
                board.ClearHighTile(tiles);
                Turn.unit.gameObject.transform.position = Turn.prevTile.pos;
                Turn.hasMoved = false;

                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                Turn.prevTile = board.GetTile(cellPosition);
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Turn.unit.gameObject.transform.position = Turn.prevTile.pos;
            }
        }
        else
        {
            // board.ClearHighTile(tiles);
            Turn.unit = null;
            StateMachineController.instance.ChangeTo<TurnBeginState>();
        }
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        
    }

    /**********************************************************
    * 움직일 수 있는 범위 표시
    ***********************************************************/
    public void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.prevTile.pos), ValidateMovement);
        board.ShowHighTile(tiles);
    }


    /**********************************************************
    * 움직일 수 있는 범위 검색
    ***********************************************************/
    // test 이것들은 어디에 있어야할까
    public bool ValidateMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1; // 올리는 이유
        //to.distance = from.distance+from.moveCost;

        if (to.content != null || to.distance > range)
        {
            // 비어있지않고 범위보다 넓으면
            return false;
        }
        return true;
    }
}
