/**********************************************************
* Ÿ���� ������ �� Ȯ���ϴ� state
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

        if(!Turn.isHumanTurn)
        {
            StartCoroutine(AISkillTargeting());
            return;
        }


        if (Turn.skill.data.isAOE)
        {
            Debug.Log($"{GetType()} - ����Ÿ�� ������");
            ShowAOERange();
        }

        AddTarget();
        
        uiController.EnableCanvas();
        
        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        uiController.DisableCanvas();

        //Turn.selectedPos = Turn.unit.pos; // �̰� �� �ϴ���

        if (tiles != null)
        {
            board.ClearTile();
        }

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    // ui Ȯ�� ��� 


    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        // �� �� ����     
    }
    public override void TouchEnd(Vector2 screenPosition, float time)
    {

    }


    /**********************************************************
    * Ÿ�� �ֱ�
    ***********************************************************/
    private void AddTarget()
    {
        if (Turn.targets != null)
        {
            Turn.targets.Clear();
        }

        if (!Turn.skill.data.isAOE)
        {
            Turn.targets.Add(board.mainTiles[Turn.selectedPos].content.GetComponent<Unit>());
        }
        else
        {
            foreach (var kvp in board.aimingTiles)
            {
                if (board.mainTiles[kvp.Value.pos].content != null)
                {
                    Debug.Log($"{GetType()} - {board.mainTiles[kvp.Value.pos].content} �ֱ�");
                    Debug.Log($"{GetType()} - {board.mainTiles[Turn.selectedPos].content}");
                    Turn.targets.Add(board.mainTiles[kvp.Value.pos].content.GetComponent<Unit>());
                }
            }
        }
    }

    /**********************************************************
    * ������ų ���� ���̵���
    ***********************************************************/
    private void ShowAOERange()
    {
        if(Turn.skill.data.AOERange.Equals(0))
        {
            tiles = searchMachine.SearchRange(Turn.unit.pos, Turn.skill.data, false);
        }
        else
        {
            tiles = searchMachine.SearchRange(Turn.selectedPos, Turn.skill.data, true);
        }

        board.ShowHighlightTile(tiles, 1);
        board.ShowAimingTile(tiles, (int)Turn.skill.data.affectType);
    }



    private IEnumerator AISkillTargeting()
    {
        Debug.Log($"{GetType()} - AI : STS");

        if (Turn.skill.data.isAOE)
        {
            ShowAOERange();
        }

        AddTarget();

        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


}
