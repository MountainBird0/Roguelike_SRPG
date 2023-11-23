/**********************************************************
* unit�� �ൿ�� �����ϴ� State
***********************************************************/
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

    private void OnEnable()
    {
        raycaster = uiController.raycaster;
    }

    public override void Enter()
    {
        base.Enter();

        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AIChooseAction());
        }  
        else
        {// ���߿� ���Ϳ� ui ������
            uiController.EnableCanvas();
        }

        if (!Turn.hasMoved)
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

            Turn.hasMoved = false;
            Turn.unit.gameObject.transform.position = Turn.originTile.pos; // ������ġ�� ���ƿ�
 
            if (board.mainTiles[cellPosition].content != null &&
                board.mainTiles[cellPosition].content.GetComponent<Unit>().playerType.Equals(PlayerType.HUMAN)
                && !board.mainTiles[cellPosition].content.GetComponent<Unit>().isTurnEnd) // �������� �̹� ������ �ִٸ�
            {
                ChangeUnit(cellPosition);
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
            Turn.hasMoved = false;
            board.ClearTile();

            var slot = ob.GetComponent<BattleSkillSlot>();
            Turn.skillSlotNum = slot.slotNum;

            if(slot.id.Equals(-1))
            {
                return;
            }

            Turn.currentSkill = DataManager.instance.defaultSkillStats[slot.id];

            if (Turn.currentSkill.isDirectional)
            {
                StateMachineController.instance.ChangeTo<ArrowSelectionState>();
            }
            else
            {
                StateMachineController.instance.ChangeTo<SkillSelectedState>();
            }
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
        tiles = board.Search(board.GetTile(Turn.originTile.pos), Turn.unit.stats.MOV, board.ISMovable);
        board.ShowHighlightTile(tiles, 0);
    }

    /**********************************************************
    * AI
    ***********************************************************/
    private IEnumerator AIChooseAction()
    {
        aiController.Evaluate();

        yield return new WaitForSeconds(1f);

        if (aiController.aiPlan == null)
        {
            Debug.Log($"{GetType()} - ���߿� �׳� �̵�����");
            StateMachineController.instance.ChangeTo<TurnEndState>();
        }
        else
        {
            aiPlan = aiController.aiPlan;
            Debug.Log($"{GetType()} - 2{aiPlan.movePos}");
            if (!aiPlan.movePos.Equals(Turn.unit.currentPos))
            {
                StateMachineController.instance.ChangeTo<MoveSequenceState>();
            }
            else
            {
                if (aiPlan.direction.Equals(Vector3Int.zero))
                {
                    StateMachineController.instance.ChangeTo<SkillSelectedState>();
                }
                else
                {
                    StateMachineController.instance.ChangeTo<ArrowSelectionState>();
                }
            }
        }
    }
}
