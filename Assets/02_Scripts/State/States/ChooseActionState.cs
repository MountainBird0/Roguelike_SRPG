/**********************************************************
* unit�� �ൿ�� �����ϴ� State
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

    // �����ϼ��ִ� ���� ǥ��
    // �����̱�
    // ������ ��ġ���� 
    // �����ӽ�ų ��� ����
    // �� �ѱ�� ��ư

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
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // Debug.Log($"{GetType()} - ��ġ ����");

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);

        //Debug.Log($"{GetType()} - ui ��ġ�ߴ��� {EventSystem.current.IsPointerOverGameObject()}");
        // UI ������ ��
        if (clickResults.Count != 0)
        {
            var ob = clickResults[0].gameObject;
            if (ob.CompareTag("EquipSlot"))
            {
                board.ClearTile();

                // selectedTile�� ���� ���� ��ġ ����
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
        // �̵�Ÿ�� - �Ķ�, ��ų����Ÿ�� - ���, ������ųŸ��  - ����
        //                  Ÿ���� ǥ��          ���� ��ü ������


        // 1. �������� �ƹ����� �ƴҶ�
        // -> �׳� TurnBeginState�� 

        // 2. �������� �ٸ� unit �� ��
        // -> Turn.unit �װɷ� �ٲٰ� TurnBeginState��

        // 3. �������� �ڱ��ڽ� �� ��
        // -> �������� ǥ��??

        // 4. �������� ������ �� �ִ� ��Ҷ��
        // -> MoveSequenceState��

        // 5. ��ų ������ ������
        // -> ���� unit�� position(selectedTile)�� content�� unit �����ؾ��ҵ�?

        Vector3Int cellPosition = GetCellPosition(screenPosition);
        // �̵������� Ÿ���� ��ġ�ߴٸ�
        if (board.highlightTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
        }
        // �̵� �Ұ����� ���� ��ġ�ߴٸ�
        else if (board.mainTiles.ContainsKey(cellPosition))
        {
            board.ClearTile();
            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.currentTile.pos; // ������ġ�� ���ƿ�

            // �������� �̹� ������ �ִٸ�
            if (board.mainTiles[cellPosition].content) // ������ Human�� ���� �۵��ϴ� ���� �߰��ϱ�
            {
                Turn.currentTile = board.GetTile(cellPosition);
                Turn.selectedTile = Turn.currentTile;
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Turn.unit = null;
                Turn.currentTile = null; // ���ص��ǳ�
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
    * ������ �� �ִ� ���� ǥ��
    ***********************************************************/
    private void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.currentTile.pos), Turn.unit.stats.MOV, ISMovement);
        board.ShowMovableTile(tiles);
    }

    /**********************************************************
    * ������ �� �ִ� ���� �˻�
    ***********************************************************/
    // test �̰͵��� ��� �־���ұ�
    private bool ISMovement(TileLogic from, TileLogic to, int range)
    {
        to.distance = from.distance + 1;

        return (to.content == null && to.distance <= range);
    }
}
