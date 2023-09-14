/**********************************************************
* �� ��ų�� Ÿ���� ������ �� �ִ� State
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectedState : State
{
    private List<TileLogic> tiles;
    private SkillSelectionUIController uiController = BattleMapUIManager.instance.skillSelectionUIController;
    private RangeSearchMachine searchMachine;

    private void OnEnable()
    {
        searchMachine = BattleMapManager.instance.rangeSearchMachine;
    }

    public override void Enter()
    {
        base.Enter();
        uiController.EnableCanvas();
        ShowRangeTile();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        uiController.DisableCanvas();

        if(tiles != null)
        {
            board.ClearTile();
        }

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    // ��ų ���� ������ ���� ����, �÷��̾� ���⵵ �ʿ�
    // Ÿ���� ������ ���� ǥ��

    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // ���⵵ ui���� �����°� �߰�
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (board.aimingTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            Debug.Log($"{GetType()} - �������ִ°� ����");
            StateMachineController.instance.ChangeTo<SkillTargetingState>();

        }

        // Ÿ���� �����ϰų�

        // �� Ÿ�����κ��� ������ �� �ִ°�

        // ��¶�� skillTargetState�� ���� ���� �ϱ�
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * ���õ� ��ų�� ���� ǥ��
    ***********************************************************/
    private void ShowRangeTile()
    {
        tiles = searchMachine.SearchRange(board, Turn.selectedTile.pos, Turn.currentSkill.range);
        board.ShowHighlightTile(tiles, 2);
        board.ShowAimingTile(tiles, 2);
        // constant�� ���⼭ ��ų ���� �����ְ�
        // �����̸�



        // ���� ���ؾ��ϴ� ��ų
        // �׳� �ڱ��ڽ� ������ų
        // ������ ��ġ ������ų
    }





    /**********************************************************
    * ��ų ���� �˻�
    ***********************************************************/
    //private bool ConstantRange(TileLogic from, TileLogic to)
    //{
    //    to.distance = from.distance + 1;
    //    int range = Turn.currentSkill.range;

    //    return to.distance <= range;
    //}



}
