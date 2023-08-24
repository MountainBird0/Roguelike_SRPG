using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseActionState : State
{
    private ChooseActionUIController uiController = BattleMapUIManager.instance.ChooseActionUIController;
    
    private List<TileLogic> tiles;

    private GraphicRaycaster raycaster;
    private PointerEventData clickData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> clickResults = new();

    // 움직일수있는 범위 표시
    // 움직이기
    // 움직인 위치에서 
    // 움직임스킬 사용 상태
    // 턴 넘기기 버튼

    private void OnEnable()
    {
        raycaster = uiController.raycaster;
    }


    public override void Enter()
    {
        base.Enter();

        uiController.EnableCanvas();

        if(!Turn.isMoving)
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
        // UI 눌렀을 때
        if (EventSystem.current.IsPointerOverGameObject())
        {
            clickResults.Clear();
            clickData.position = screenPosition;
            raycaster.Raycast(clickData, clickResults);

            var ob = clickResults[0].gameObject;
            if (ob.CompareTag("EquipSlot"))
            {
                board.ClearHighTile(tiles);

                // selectedTile에 실제 유닛 위치 박음
                board.mainTiles[Turn.selectedTile.pos].content = board.mainTiles[Turn.prevTile.pos].content;
                board.mainTiles[Turn.prevTile.pos].content = null;
                Turn.prevTile = Turn.selectedTile;

                Turn.isMoving = false;

                Turn.currentSkill = DataManager.instance.defaultSkillStats[ob.GetComponent<SkillSlot>().id];
                StateMachineController.instance.ChangeTo<SkillSelectionState>();

                return;
            }
        }

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
            board.ClearHighTile(tiles);
            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.prevTile.pos;

            // 누른곳에 이미 유닛이 있다면
            if (board.mainTiles[cellPosition].content) // 내 유닛일때만 추가
            {
                Turn.prevTile = board.GetTile(cellPosition);
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Turn.unit = null;
                Turn.prevTile = null; // 안해도되나
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
        //else
        //{
        //    // board.ClearHighTile(tiles);
        //    Turn.unit = null;
        //    StateMachineController.instance.ChangeTo<TurnBeginState>();
        //}
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        
    }

    /**********************************************************
    * 움직일 수 있는 범위 표시
    ***********************************************************/
    private void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.prevTile.pos), ISMovement);
        board.ShowHighTile(tiles);
    }


    /**********************************************************
    * 움직일 수 있는 범위 검색
    ***********************************************************/
    // test 이것들은 어디에 있어야할까
    private bool ISMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        int range = Turn.unit.stats.MOV;
        Debug.Log($"{GetType()} - {to.content}");

        return (to.content == null && to.distance <= range);
    }
}
