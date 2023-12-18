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
    private AIController aiController;

    private List<TileLogic> tiles;

    private void OnEnable()
    {
        aiController = BattleMapManager.instance.aiController;
    }

    public override void Enter()
    {
        base.Enter();

        if (Turn.isMoving)
        {
            ShowMoveableTile();
            Turn.isMoving = false;
        }
        
        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AIChooseAction());
            return;
        }  

        uiController.EnableCanvas();
        
        InputManager.instance.OnStartTouch += TouchStart;
    }

    public override void Exit()
    {
        base.Exit();

        uiController.DisableCanvas();
        if(!Turn.isMoving)
        {
            board.ClearTile();
        }

        InputManager.instance.OnStartTouch -= TouchStart;
    }

    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        InputManager.instance.RaycastUI(uiController.raycaster);

        if (InputManager.instance.clickResults.Count != 0) // UI ������ ��
        {
            SelectSkill();
            return;
        }

        Vector3Int cellPosition = GetCellPosition(screenPosition);       
        if (board.highlightTiles.ContainsKey(cellPosition)) // �̵������� Ÿ���� ��ġ�ߴٸ�
        { 
            MoveUnit(cellPosition);
            return;
        }  
        else if (board.mainTiles.ContainsKey(cellPosition)) // �̵� �Ұ����� ���� ��ġ�ߴٸ�
        {
            ReturnUnit();

            var selectedUnit = board.mainTiles[cellPosition].content;

            if (selectedUnit != null)
            {
                var unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                if (unit.playerType == PlayerType.HUMAN && !unit.isTurnEnd)
                {
                    ChangeUnit(cellPosition);
                }
                else
                {
                    Turn.unit = null;
                    StateMachineController.instance.ChangeTo<TurnBeginState>();
                }
            }
            else
            {
                Turn.unit = null;
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
    }

    /**********************************************************
    * ��ų ui������ �׿� �´� ���·� �̵�
    ***********************************************************/
    private void SelectSkill()
    {
        var ob = InputManager.instance.clickResults[0].gameObject;
        if (ob.CompareTag("EquipSlot"))
        {
            var slot = ob.GetComponent<BattleSkillSlot>();

            if(slot.slotNum != -1)
            {
                SkillSetting(slot.slotNum);
            }
        }      
    }

    /**********************************************************
    * ��ų ����
    ***********************************************************/
    private void SkillSetting(int slotNum)
    {
        Turn.skill = Turn.unit.skills[slotNum].GetComponent<Skill>();

        if (Turn.skill.data.isDirectional)
        {
            StateMachineController.instance.ChangeTo<ArrowSelectionState>();
        }
        else
        {
            StateMachineController.instance.ChangeTo<SkillSelectedState>();
        }
    }

    /**********************************************************
    * ���� �̵�
    ***********************************************************/
    private void MoveUnit(Vector3Int cellPosition)
    {
        Turn.isMoving = true;
        Turn.selectedPos = cellPosition;

        // ReturnUnit();

        StateMachineController.instance.ChangeTo<MoveSequenceState>();     
    }
    /**********************************************************
    * ���� ���ڸ���
    ***********************************************************/
    private void ReturnUnit()
    {
        Turn.unit.SetPosition(Turn.originPos, board);
    }

    /**********************************************************
    * �ٸ� �������� ����
    ***********************************************************/
    private void ChangeUnit(Vector3Int cellPosition)
    {
        Turn.selectedPos = cellPosition;

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * ������ �� �ִ� ���� ǥ��
    ***********************************************************/
    private void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.originPos), Turn.unit.stats.MOV, board.ISMovable);
        board.ShowHighlightTile(tiles, 0);
    }


    /**********************************************************
    * AI
    ***********************************************************/
    private IEnumerator AIChooseAction()
    {
        Debug.Log($"{GetType()} - AI : CAS");
        
        AIPlan plan = aiController.currentPlan;

        if(plan == null)
        {
            Debug.Log($"{GetType()} - �÷����� ����");
            plan = aiController.Evaluate();

            yield return new WaitForSeconds(1f);

            if (plan == null)
            {
                Debug.Log($"{GetType()} - ��ȹ ��ã��");
 
                StateMachineController.instance.ChangeTo<TurnEndState>();
                yield break;
            }
        }

        //if (!Turn.isMoving && (plan.movePos != Turn.unit.pos))
        if (plan.movePos != Turn.unit.pos)
        {
            Debug.Log($"{GetType()} - �����̷���");
            MoveUnit(plan.movePos);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
            yield break;
        }

        if(plan.skill == null)
        {
            aiController.currentPlan = null;
            StateMachineController.instance.ChangeTo<TurnEndState>();
            yield break;
        }

        for (int i = 0; i < Turn.unit.skills.Count; i++)
        {
            if (Turn.unit.skills[i].name.Equals(plan.skill.name))
            {
                Turn.direction = plan.direction;
                Turn.selectedPos = plan.targetPos;
                aiController.currentPlan = null;
                SkillSetting(i);
                break;
            }
        }      
    }
}
