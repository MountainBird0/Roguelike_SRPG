/**********************************************************
* Ÿ���� ������ �� Ȯ���ϴ� state
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
        uiController.EnableCanvas();

        // ������ġ ������ �����Ƿ� �װ��� ���� �޾ƿ���

        // �����̸� ���� ���� ui����

        // ������ AOE Range �̿�

        if (!Turn.currentSkill.AOERange.Equals(0))
        {
            tiles = searchMachine.SearchRange(board, Turn.selectedTile.pos, Turn.currentSkill.AOERange);

            // Debug.Log($"{GetType()} - �� �ߴ��� {(int)(AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true)}");

            board.ShowHighlightTile(tiles, 1);
            board.ShowAimingTile(tiles, (int)(AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true));
        }

       

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        uiController.DisableCanvas();

        Turn.selectedTile = Turn.currentTile;

        if (tiles != null)
        {
            board.ClearTile();
        }

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    // ui Ȯ�� ��� 


    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // �� �� ����




       
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }
}
