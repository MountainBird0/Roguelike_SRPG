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

        if (!Turn.isHumanTurn)
        {
            StartCoroutine(AIArrowSelected());
        }

        board.ShowArrowTile(Turn.currentTile.pos);
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
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 방향 타일중에 하나 누르면 그 방향 저장하고 SkillSelectionState로
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if(board.aimingTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = Turn.currentTile;

            Vector3Int check = cellPosition - Turn.currentTile.pos;

            Turn.direction = cellPosition - Turn.currentTile.pos;

            StateMachineController.instance.ChangeTo<SkillSelectedState>();
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 방향 고르기 
    ***********************************************************/
    private void SelectArrow(Vector3Int cellPosition)
    {
        // Turn.selectedTile = Turn.currentTile;

        Turn.direction = cellPosition - Turn.currentTile.pos;

        StateMachineController.instance.ChangeTo<SkillSelectedState>();
    }

    private IEnumerator AIArrowSelected()
    {
        board.ShowArrowTile(aiPlan.movePos);

        yield return new WaitForSeconds(1f);
        StateMachineController.instance.ChangeTo<SkillSelectedState>();
    }

}
