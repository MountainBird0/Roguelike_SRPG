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

        ShowRangeTile();
        
        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AISkillSelected());
            return;
        }

        uiController.EnableCanvas();

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

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // 여기도 ui먼저 누르는거 추가
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (board.aimingTiles.ContainsKey(cellPosition))
        {
            Turn.selectedPos = cellPosition;
            StateMachineController.instance.ChangeTo<SkillTargetingState>();
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * 선택된 스킬의 범위 표시
    ***********************************************************/
    private void ShowRangeTile()
    {
        tiles = searchMachine.SearchRange(Turn.unit.pos, Turn.skill.data, false);
        board.ShowHighlightTile(tiles, 2);
        board.ShowAimingTile(tiles, 2);
    }

    /**********************************************************
    * AI
    ***********************************************************/
    private IEnumerator AISkillSelected()
    {
        Debug.Log($"{GetType()} - AI : SSS");
        Debug.Log($"{GetType()} - 선택한 스킬 {Turn.skill.data.name}");

        yield return new WaitForSeconds(1);

        StateMachineController.instance.ChangeTo<SkillTargetingState>();
    }
}
