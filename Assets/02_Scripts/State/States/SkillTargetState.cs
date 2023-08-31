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

    // ui 확인 취소 


    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 단일이면 유닛 정보 ui나옴

        // 범위면 AOE Range 이용



       
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }
}
