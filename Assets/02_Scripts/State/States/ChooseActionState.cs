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
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // Debug.Log($"{GetType()} - ��ġ ����");
        //Debug.Log($"{GetType()} - ui ��ġ�ߴ��� {EventSystem.current.IsPointerOverGameObject()}");

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);
       
        if (clickResults.Count != 0) // UI ������ ��
        {
            SelectSkill();
            return;
        }
        // �̵�Ÿ�� - �Ķ�, ��ų����Ÿ�� - ���, ������ųŸ��  - ����
        //                  Ÿ���� ǥ��          ���� ��ü ������

        Vector3Int cellPosition = GetCellPosition(screenPosition);
        
        if (board.highlightTiles.ContainsKey(cellPosition)) // �̵������� Ÿ���� ��ġ�ߴٸ�
        { 
            MoveUnit(cellPosition);
            return;
        }  
        else if (board.mainTiles.ContainsKey(cellPosition)) // �̵� �Ұ����� ���� ��ġ�ߴٸ�
        { 
            board.ClearTile();

            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.originTile.pos; // ������ġ�� ���ƿ�
 
            if (board.mainTiles[cellPosition].content)
            { // �������� �̹� ������ �ִٸ� //������ Human�� ���� �۵��ϴ� ���� �߰��ϱ�
                ChangeUnit(cellPosition);
                return;
            }
            else
            { // ���� ���°� ����
                Turn.unit = null;
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * ��ų ui������ �׿� �´� ���·� �̵�
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
    * ���� �̵�
    ***********************************************************/
    private void MoveUnit(Vector3Int cellPosition)
    {
        Turn.currentTile = new TileLogic(cellPosition);
        Turn.selectedTile = Turn.currentTile;

        StateMachineController.instance.ChangeTo<MoveSequenceState>();
    }

    /**********************************************************
    * �ٸ� �������� ����
    ***********************************************************/
    private void ChangeUnit(Vector3Int cellPosition)
    {
        Turn.selectedTile = board.GetTile(cellPosition);

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * ������ �� �ִ� ���� ǥ��
    ***********************************************************/
    private void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.originTile.pos), Turn.unit.stats.MOV, ISMovement);
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
