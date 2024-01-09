/**********************************************************
* unit의 행동을 결정하는 State
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChooseActionState : State
{
    private ChooseActionUIController uiController = BattleMapUIManager.instance.chooseActionUIController;
    private AIController aiController;

    private List<TileLogic> tiles;

    private IEnumerator uiTouchCoroutine;
    private int skillNum = -1;



    private void OnEnable()
    {
        aiController = BattleMapManager.instance.aiController;
        
    }

    public override void Enter()
    {
        base.Enter();

        skillNum = -1;
        
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
        InputManager.instance.OnEndTouch += TouchEnd;
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
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        uiTouchCoroutine = TouchSkillIcon();
        uiController.isHovor = false;

        InputManager.instance.RaycastUI(uiController.raycaster);

        Debug.Log($"{GetType()} - 아이콘 수 {InputManager.instance.clickResults.Count}");

        if (InputManager.instance.clickResults.Count != 0) // UI 눌렀을 때
        {
            Debug.Log($"{GetType()} - 터치스타트");
            StartCoroutine(uiTouchCoroutine);
            return;
        }

        Vector3Int cellPosition = GetCellPosition(screenPosition);       
        if (board.highlightTiles.ContainsKey(cellPosition)) // 이동가능한 타일을 터치했다면
        { 
            MoveUnit(cellPosition);
            return;
        }  
        else if (board.mainTiles.ContainsKey(cellPosition)) // 이동 불가능한 곳을 터치했다면
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

    public override void TouchEnd(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - 터치끝");

        if(uiTouchCoroutine != null)
        {
            StopCoroutine(uiTouchCoroutine);
        }
        uiController.DisableHovor(skillNum);

        if (uiController.isHovor == false && skillNum != -1)
        {
            SkillSetting(skillNum);
        }
    }

    /**********************************************************
    * ui창 눌렀다면
    ***********************************************************/
    private IEnumerator TouchSkillIcon()
    {
        Debug.Log($"{GetType()} - 스킬터치 시작");
        var ob = InputManager.instance.clickResults[0].gameObject;
        if(ob.CompareTag("EquipSlot"))
        {
            var slot = ob.GetComponent<BattleSkillSlot>();
            if (slot.slotNum != -1)
            {
                skillNum = slot.slotNum;
                Debug.Log($"{GetType()} 코루틴 스킬 숫자 - {skillNum}");

                yield return new WaitForSeconds(1.0f);

                uiController.isHovor = true;
                Debug.Log($"{GetType()} - 호버 생김");

                uiController.EnableHovor(skillNum);
            }
        }
    }

    /**********************************************************
    * 스킬 세팅
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
    * 유닛 이동
    ***********************************************************/
    private void MoveUnit(Vector3Int cellPosition)
    {
        Turn.isMoving = true;
        Turn.selectedPos = cellPosition;

        // ReturnUnit();

        StateMachineController.instance.ChangeTo<MoveSequenceState>();     
    }
    /**********************************************************
    * 유닛 제자리로
    ***********************************************************/
    private void ReturnUnit()
    {
        Turn.unit.SetPosition(Turn.originPos, board);
    }

    /**********************************************************
    * 다른 유닛으로 변경
    ***********************************************************/
    private void ChangeUnit(Vector3Int cellPosition)
    {
        Turn.selectedPos = cellPosition;

        StateMachineController.instance.ChangeTo<TurnBeginState>();
    }

    /**********************************************************
    * 움직일 수 있는 범위 표시
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
            Debug.Log($"{GetType()} - 플랜새로 만듬");
            plan = aiController.Evaluate();

            yield return new WaitForSeconds(1f);

            if (plan == null)
            {
                Debug.Log($"{GetType()} - 계획 못찾음");
 
                StateMachineController.instance.ChangeTo<TurnEndState>();
                yield break;
            }
        }

        //if (!Turn.isMoving && (plan.movePos != Turn.unit.pos))
        if (plan.movePos != Turn.unit.pos)
        {
            Debug.Log($"{GetType()} - 움직이러감");
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
