/**********************************************************
* �� ��ų�� Ÿ���� ������ �� �ִ� State
***********************************************************/
using System.Collections;
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

        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AISkillSelected());
        }

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
        tiles = searchMachine.SearchRange(Turn.selectedTile.pos, Turn.currentSkill, false);
        board.ShowHighlightTile(tiles, 2);
        board.ShowAimingTile(tiles, 2);
    }

    /**********************************************************
    * AI
    ***********************************************************/
    private IEnumerator AISkillSelected()
    {
        tiles = searchMachine.SearchRange(aiPlan.targetPos, aiPlan.skill.data, false);
        board.ShowHighlightTile(tiles, 2);
        board.ShowAimingTile(tiles, 2);

        yield return new WaitForSeconds(1);

        StateMachineController.instance.ChangeTo<SkillTargetingState>();
    }







}
