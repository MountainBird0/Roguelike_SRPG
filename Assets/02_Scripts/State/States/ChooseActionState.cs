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
    private ChooseActionUIController uiController = BattleMapUIManager.instance.ChooseActionUIController;
    private AIController aiController;

    private List<TileLogic> tiles;

    private GraphicRaycaster raycaster;
    private PointerEventData clickData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> clickResults = new();

    private void OnEnable()
    {
        raycaster = uiController.raycaster;
        aiController = BattleMapManager.instance.aiController;
    }

    public override void Enter()
    {
        base.Enter();

        if (!Turn.isMoving)
        {
            ShowMoveableTile();
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
    private void TouchStart(Vector2 screenPosition, float time)
    {
        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);
       
        if (clickResults.Count != 0) // UI 눌렀을 때
        {
            SelectSkill();
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

            if(!board.mainTiles[cellPosition].content)
            {
                Turn.unit = null;
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else if(board.mainTiles[cellPosition].content.GetComponent<Unit>().playerType.Equals(PlayerType.HUMAN)
                && !board.mainTiles[cellPosition].content.GetComponent<Unit>().isTurnEnd)
            {
                ChangeUnit(cellPosition);
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

            var slot = ob.GetComponent<BattleSkillSlot>();

            if(slot.id.Equals(-1)) // 나중에 다른방식으로 수정
            {
                return;
            }

            SkillSetting(slot.slotNum);
        }      
    }

    /**********************************************************
    * 스킬 세팅
    ***********************************************************/
    private void SkillSetting(int slotNum)
    {
        Turn.skillSlotNum = slotNum;
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
        Debug.Log($"{GetType()} - 유닛위치 콘텐츠 {board.mainTiles[Turn.unit.pos].content}");
        board.mainTiles[cellPosition].content = board.mainTiles[Turn.unit.pos].content;
        board.mainTiles[Turn.unit.pos].content = null;

        Turn.currentPos = cellPosition;
        Turn.selectedPos = Turn.currentPos;
        Turn.unit.pos = Turn.currentPos;
        Turn.isMoving = true;

        StateMachineController.instance.ChangeTo<MoveSequenceState>();     
    }
    /**********************************************************
    * 유닛 제자리로
    ***********************************************************/
    private void ReturnUnit()
    {
        if(!Turn.originPos.Equals(Turn.unit.pos))
        {
            board.mainTiles[Turn.originPos].content = board.mainTiles[Turn.unit.pos].content;
            board.mainTiles[Turn.unit.pos].content = null;
        }

        Turn.unit.pos = Turn.originPos;
        Turn.unit.gameObject.transform.position = Turn.originPos; // 원래위치로 돌아옴

        Turn.isMoving = false;
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
        
        AIPlan plan = aiController.currentAiPlan;

        if(plan == null)
        {
            Debug.Log($"{GetType()} - 플랜새로 만듬");
            plan = aiController.Evaluate();
        }

        yield return new WaitForSeconds(1f);

        if (plan == null)
        {
            Debug.Log($"{GetType()} - 계획 못찾음");
            StateMachineController.instance.ChangeTo<TurnEndState>();
            yield break;
        }

        if (!Turn.isMoving)
        {
            Debug.Log($"{GetType()} - 움직이러감");
            MoveUnit(plan.movePos);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
            yield break;
        }

        

        for (int i = 0; i < Turn.unit.skills.Count; i++)
        {
            if (Turn.unit.skills[i].name.Equals(plan.skill.name))
            {
                Turn.skillSlotNum = i;
                Turn.direction = plan.direction;
                Turn.selectedPos = plan.targetPos;
                Turn.isMoving = false;
                aiController.currentAiPlan = null;
                SkillSetting(i);
                break;
            }
        }      
    }
}
