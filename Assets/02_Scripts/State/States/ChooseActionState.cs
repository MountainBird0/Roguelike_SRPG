/**********************************************************
* unit의 행동을 결정하는 State
***********************************************************/
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

    private void OnEnable()
    {
        raycaster = uiController.raycaster;
    }

    public override void Enter()
    {
        base.Enter();

        uiController.EnableCanvas();

        if (!Turn.isMoving)
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

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // Debug.Log($"{GetType()} - 터치 시작");
        //Debug.Log($"{GetType()} - ui 터치했는지 {EventSystem.current.IsPointerOverGameObject()}");

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);
       
        if (clickResults.Count != 0) // UI 눌렀을 때
        {
            SelectSkill();
            return;
        }
        // 이동타일 - 파랑, 스킬범위타일 - 노랑, 범위스킬타일  - 빨강
        //                  타게팅 표시          유닛 자체 깜빡임

        Vector3Int cellPosition = GetCellPosition(screenPosition);
        
        if (board.highlightTiles.ContainsKey(cellPosition)) // 이동가능한 타일을 터치했다면
        { 
            MoveUnit(cellPosition);
            return;
        }  
        else if (board.mainTiles.ContainsKey(cellPosition)) // 이동 불가능한 곳을 터치했다면
        { 
            board.ClearTile();

            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.originTile.pos; // 원래위치로 돌아옴
 
            if (board.mainTiles[cellPosition].content)
            { // 누른곳에 이미 유닛이 있다면 //유닛이 Human일 때만 작동하는 조건 추가하기
                ChangeUnit(cellPosition);
                return;
            }
            else
            { // 유닛 없는곳 누름
                Turn.unit = null;
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 스킬 ui누르면 그에 맞는 상태로 이동
    ***********************************************************/
    private void SelectSkill()
    {
        var ob = clickResults[0].gameObject;
        if (ob.CompareTag("EquipSlot"))
        {
            Turn.isMoving = false;
            board.ClearTile();

            var slot = ob.GetComponent<BattleSkillSlot>();
            Turn.slotNum = slot.slotNum;
            Turn.currentSkill = DataManager.instance.defaultSkillStats[slot.id];

            if (Turn.currentSkill.isDirectional)
            {
                StateMachineController.instance.ChangeTo<ArrowSelectionState>();
            }
            else
            {
                StateMachineController.instance.ChangeTo<SkillSelectedState>();
            }
            return;
        }
    }

    /**********************************************************
    * 유닛 이동
    ***********************************************************/
    private void MoveUnit(Vector3Int cellPosition)
    {
        Turn.currentTile = new TileLogic(cellPosition);
        Turn.selectedTile = Turn.currentTile;

        StateMachineController.instance.ChangeTo<MoveSequenceState>();
    }

    /**********************************************************
    * 다른 유닛으로 변경
    ***********************************************************/
    private void ChangeUnit(Vector3Int cellPosition)
    {
        Turn.selectedTile = board.GetTile(cellPosition);

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * 움직일 수 있는 범위 표시
    ***********************************************************/
    private void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.originTile.pos), Turn.unit.stats.MOV, ISMovement);
        board.ShowMovableTile(tiles);
    }

    /**********************************************************
    * 움직일 수 있는 범위 검색
    ***********************************************************/
    // test 이것들은 어디에 있어야할까
    private bool ISMovement(TileLogic from, TileLogic to, int range)
    {
        to.distance = from.distance + 1;

        return (to.content == null && to.distance <= range);
    }
}
