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
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // ���� Ÿ���߿� �ϳ� ������ �� ���� �����ϰ� SkillSelectionState��
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
    * ���� ���� 
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
