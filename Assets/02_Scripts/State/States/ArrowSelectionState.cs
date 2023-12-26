using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionState : State
{
    private ArrowSelectionUIController uiController = BattleMapUIManager.instance.arrowSelectionUIController;

    public override void Enter()
    {
        base.Enter();

        board.ShowArrowTile(Turn.unit.pos);
        
        if (!Turn.isHumanTurn)
        {
            StartCoroutine(AIArrowSelected());
            return;
        }

        uiController.EnableCanvas();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();

        board.ClearArrowTile();
        uiController.DisableCanvas();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }


    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        // 방향 타일중에 하나 누르면 그 방향 저장하고 SkillSelectionState로
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if(board.aimingTiles.ContainsKey(cellPosition))
        {
            SelectArrow(cellPosition);
        }
    }
    public override void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 방향 고르기 
    ***********************************************************/
    private void SelectArrow(Vector3Int cellPosition)
    {
        Turn.direction = cellPosition - Turn.unit.pos;

        if(Turn.skill.data.isAOE)
        {

            StateMachineController.instance.ChangeTo<SkillTargetingState>();
        }
        else
        {
            StateMachineController.instance.ChangeTo<SkillSelectedState>();

        }
    }

    private IEnumerator AIArrowSelected()
    {
        yield return new WaitForSeconds(1f);
        
        SelectArrow(Turn.direction);
    }
}
