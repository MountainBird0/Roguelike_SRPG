using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSelectionState : State
{
    private ArrowSelectionUIController uiController = BattleMapUIManager.instance.arrowSelectionUIController;

    public override void Enter()
    {
        base.Enter();

        board.ShowArrowTile(Turn.selectedTile.pos);
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
            Turn.direction = cellPosition - Turn.selectedTile.pos;

            StateMachineController.instance.ChangeTo<SkillSelectionState>();
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

  

}
