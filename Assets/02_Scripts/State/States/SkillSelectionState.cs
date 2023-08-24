/**********************************************************
* ����� ��ų�� �� State
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectionState : State
{
    private List<TileLogic> tiles;
    private SkillSelectionUIController uiController = BattleMapUIManager.instance.skillSelectionUIController;

    public override void Enter()
    {
        base.Enter();
        uiController.EnableCanvas();
        ShowSkillTile();
    }

    public override void Exit()
    {
        base.Exit();
        board.ClearHighTile(tiles);
        uiController.DisableCanvas();
    }

    // ��ų ���� ������ ���� ����, �÷��̾� ���⵵ �ʿ�
    // Ÿ���� ������ ���� ǥ��


    /**********************************************************
    * ���õ� ��ų�� ���� ǥ��
    ***********************************************************/
    private void ShowSkillTile()
    {
        RangeType rangeType = (RangeType)Turn.currentSkill.rangeType;

        switch(rangeType)
        {
            case RangeType.CONSTANT:
                tiles = board.Search(board.GetTile(Turn.selectedTile.pos), ConstantRange);
                break;


            case RangeType.LINE:
                break;


            case RangeType.CONE:
                break;


            case RangeType.INFINITE:
                break;


            case RangeType.SELF:
                break;
        }

        board.ShowHighTile(tiles);
    }


    /**********************************************************
    * ��ų ���� �˻�
    ***********************************************************/
    private bool ConstantRange(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        int range = Turn.currentSkill.range;

        return to.distance <= range;
    }



}
