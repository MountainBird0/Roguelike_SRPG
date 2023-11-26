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
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // ���⵵ ui���� �����°� �߰�
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
    * ���õ� ��ų�� ���� ǥ��
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
        Debug.Log($"{GetType()} - ������ ��ų {Turn.skill.data.name}");

        yield return new WaitForSeconds(1);

        StateMachineController.instance.ChangeTo<SkillTargetingState>();
    }
}
