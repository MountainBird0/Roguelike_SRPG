/**********************************************************
* 사용할 스킬의 타겟을 지정할 수 있는 State
***********************************************************/
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

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        board.ClearHighTile(tiles);
        uiController.DisableCanvas();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    // 스킬 범위 종류에 따라 구분, 플레이어 방향도 필요
    // 타게팅 가능한 몬스터 표시

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 타게팅 가능하거나

        // 그 타겟으로부터 범위가 또 있는거

        // 어쨋든 skillTargetState로 가서 뭐라도 하기
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

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
