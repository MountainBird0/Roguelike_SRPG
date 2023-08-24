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

    private void TouchStart(Vector2 screenPosition, float time)
    {
        // UI ������ ��
        if (EventSystem.current.IsPointerOverGameObject())
        {
            clickResults.Clear();
            clickData.position = screenPosition;
            raycaster.Raycast(clickData, clickResults);

            var ob = clickResults[0].gameObject;
            if (ob.CompareTag("EquipSlot"))
            {
                board.ClearHighTile(tiles);

                // selectedTile�� ���� ���� ��ġ ����
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

        // �̵������� Ÿ���� ��ġ�ߴٸ�
        if (board.highlightTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
        }
        // �̵� �Ұ����� ���� ��ġ�ߴٸ�
        else if (board.mainTiles.ContainsKey(cellPosition))
        {
            board.ClearHighTile(tiles);
            Turn.isMoving = false;
            Turn.unit.gameObject.transform.position = Turn.prevTile.pos;

            // �������� �̹� ������ �ִٸ�
            if (board.mainTiles[cellPosition].content) // �� �����϶��� �߰�
            {
                Turn.prevTile = board.GetTile(cellPosition);
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Turn.unit = null;
                Turn.prevTile = null; // ���ص��ǳ�
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
        tiles = board.Search(board.GetTile(Turn.prevTile.pos), ISMovement);
        board.ShowHighTile(tiles);
    }


    /**********************************************************
    * ������ �� �ִ� ���� �˻�
    ***********************************************************/
    // test �̰͵��� ��� �־���ұ�
    private bool ISMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        int range = Turn.unit.stats.MOV;
        Debug.Log($"{GetType()} - {to.content}");

        return (to.content == null && to.distance <= range);
    }
}
