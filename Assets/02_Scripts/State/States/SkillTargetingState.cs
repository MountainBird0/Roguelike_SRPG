/**********************************************************
* 타겟을 지정한 후 확인하는 state
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

        // 때린위치 정보가 있으므로 그곳의 유닛 받아오기

        // 단일이면 유닛 정보 ui나옴

        // 범위면 AOE Range 이용

        if (!Turn.currentSkill.AOERange.Equals(0))
        {
            tiles = searchMachine.SearchRange(board, Turn.selectedTile.pos, Turn.currentSkill.AOERange);

            // Debug.Log($"{GetType()} - 머 뜨는지 {(int)(AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true)}");

            board.ShowHighlightTile(tiles, 1);
            board.ShowAimingTile(tiles, (int)(AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true));
        }

        AddTarget();
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

    // ui 확인 취소 


    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 쓸 일 없음     
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }


    /**********************************************************
    * 타겟 넣기
    ***********************************************************/
    private void AddTarget()
    {
        if (Turn.targets != null)
        {
            Turn.targets.Clear();
        }

        if (Turn.currentSkill.AOERange.Equals(0))
        {
            Turn.targets.Add(board.mainTiles[Turn.selectedTile.pos].content.GetComponent<Unit>());
        }
        else
        {
            foreach (var kvp in board.aimingTiles)
            {
                if (board.mainTiles[kvp.Value.pos].content != null)
                {
                    Turn.targets.Add(board.mainTiles[kvp.Value.pos].content.GetComponent<Unit>());
                }
            }
        }
    }
}
