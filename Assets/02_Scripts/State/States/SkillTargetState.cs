using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTargetState : State
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
        uiController.EnableCanvas();

        if (!Turn.currentSkill.AOERange.Equals(1))
        {
            tiles = searchMachine.SearchRange(board, Turn.selectedTile.pos, Turn.currentSkill.AOERange);
            board.ShowSkillRangeTile(tiles);
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

    // ui Ȯ�� ��� 


    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // �����̸� ���� ���� ui����

        // ������ AOE Range �̿�



       
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }
}
