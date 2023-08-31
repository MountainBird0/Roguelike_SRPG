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

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // Debug.Log($"{GetType()} - 터치 시작");

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);

        //Debug.Log($"{GetType()} - ui 터치했는지 {EventSystem.current.IsPointerOverGameObject()}");
        // UI 눌렀을 때
        if (clickResults.Count != 0)
        {
            var ob = clickResults[0].gameObject;
            if (ob.CompareTag("EquipSlot"))
            {
                board.ClearTile();

                // selectedTile에 실제 유닛 위치 박음
                //if(!Turn.selectedTile.pos.Equals(Turn.currentTile.pos))
                //{
                //    board.mainTiles[Turn.selectedTile.pos].content = board.mainTiles[Turn.currentTile.pos].content;
                //    board.mainTiles[Turn.currentTile.pos].content = null;
                //}
                //Turn.currentTile = Turn.selectedTile;

                Turn.isMoving = false;

                Turn.currentSkill = DataManager.instance.defaultSkillStats[ob.GetComponent<SkillSlot>().id];

                if(Turn.currentSkill.isDirectional)
                {
                    StateMachineController.instance.ChangeTo<ArrowSelectionState>();
                    return;
                }
            
                StateMachineController.instance.ChangeTo<SkillSelectionState>();
                return;
            }
        }
        // 이동타일 - 파랑, 스킬범위타일 - 노랑, 범위스킬타일  - 빨강
        //                  타게팅 표시          유닛 자체 깜빡임


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

        Vector3Int cellPosition = GetCellPosition(screenPosition);
        // 이동가능한 타일을 터치했다면
        if (board.highlightTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
        }
        // 이동 불가능한 곳을 터치했다면
        else if (board.mainTiles.ContainsKey(cellPosition))
        {
            board.ClearTile();
            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.currentTile.pos; // 원래위치로 돌아옴

            // 누른곳에 이미 유닛이 있다면
            if (board.mainTiles[cellPosition].content) // 유닛이 Human일 때만 작동하는 조건 추가하기
            {
                Turn.currentTile = board.GetTile(cellPosition);
                Turn.selectedTile = Turn.currentTile;
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Turn.unit = null;
                Turn.currentTile = null; // 안해도되나
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
        tiles = board.Search(board.GetTile(Turn.currentTile.pos), Turn.unit.stats.MOV, ISMovement);
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
