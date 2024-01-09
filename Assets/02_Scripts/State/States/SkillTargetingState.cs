/**********************************************************
* 타겟을 지정한 후 확인하는 state
***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetingState : State
{
    private List<TileLogic> tiles;
    private RangeSearchMachine searchMachine;
    private SkillTargetUIController uiController;

    private void OnEnable()
    {
        uiController = BattleMapUIManager.instance.skillTargetUIController;
        searchMachine = BattleMapManager.instance.rangeSearchMachine;
    }

    public override void Enter()
    {
        base.Enter();

        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AISkillTargeting());
            return;
        }


        if (Turn.skill.data.isAOE)
        {
            Debug.Log($"{GetType()} - 범위타일 보여줌");
            ShowAOERange();
        }

        AddTarget();
        
        uiController.EnableCanvas();
        
        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        uiController.DisableCanvas();

        //Turn.selectedPos = Turn.unit.pos; // 이거 왜 하는지

        if (tiles != null)
        {
            board.ClearTile();
        }

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    // ui 확인 취소 


    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        // 쓸 일 없음     
    }
    public override void TouchEnd(Vector2 screenPosition, float time)
    {

    }


    /**********************************************************
    * 타겟 넣기
    ***********************************************************/
    private void AddTarget()
    {
        if (Turn.targets != null)
        {
            Turn.targets.Clear();
        }

        if (!Turn.skill.data.isAOE)
        {
            Turn.targets.Add(board.mainTiles[Turn.selectedPos].content.GetComponent<Unit>());
        }
        else
        {
            foreach (var kvp in board.aimingTiles)
            {
                if (board.mainTiles[kvp.Value.pos].content != null)
                {
                    Debug.Log($"{GetType()} - {board.mainTiles[kvp.Value.pos].content} 넣기");
                    Debug.Log($"{GetType()} - {board.mainTiles[Turn.selectedPos].content}");
                    Turn.targets.Add(board.mainTiles[kvp.Value.pos].content.GetComponent<Unit>());
                }
            }
        }
    }

    /**********************************************************
    * 범위스킬 범위 보이도록
    ***********************************************************/
    private void ShowAOERange()
    {
        if(Turn.skill.data.AOERange.Equals(0))
        {
            tiles = searchMachine.SearchRange(Turn.unit.pos, Turn.skill.data, false);
        }
        else
        {
            tiles = searchMachine.SearchRange(Turn.selectedPos, Turn.skill.data, true);
        }

        board.ShowHighlightTile(tiles, 1);
        board.ShowAimingTile(tiles, (int)Turn.skill.data.affectType);
    }



    private IEnumerator AISkillTargeting()
    {
        Debug.Log($"{GetType()} - AI : STS");

        if (Turn.skill.data.isAOE)
        {
            ShowAOERange();
        }

        AddTarget();

        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


}
