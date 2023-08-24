/**********************************************************
* 사용할 스킬을 고른 State
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

    // 스킬 범위 종류에 따라 구분, 플레이어 방향도 필요
    // 타게팅 가능한 몬스터 표시


    /**********************************************************
    * 선택된 스킬의 범위 표시
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
    * 스킬 범위 검색
    ***********************************************************/
    private bool ConstantRange(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        int range = Turn.currentSkill.range;

        return to.distance <= range;
    }



}
