/**********************************************************
* 고른 스킬의 타겟을 지정할 수 있는 State
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

    // 스킬 범위 종류에 따라 구분, 플레이어 방향도 필요
    // 타게팅 가능한 몬스터 표시

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 여기도 ui먼저 누르는거 추가
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (board.aimingTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            Debug.Log($"{GetType()} - 때릴수있는거 누름");
            StateMachineController.instance.ChangeTo<SkillTargetingState>();

        }

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
