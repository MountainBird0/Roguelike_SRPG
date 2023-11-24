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

        uiController.EnableCanvas();

        // ������ġ ������ �����Ƿ� �װ��� ���� �޾ƿ���

        // �����̸� ���� ���� ui����

        // ������ AOE Range �̿�

        // Turn.currentSkill.isAOE

        if (!Turn.currentSkill.AOERange.Equals(0))
        {
            tiles = searchMachine.SearchRange(Turn.selectedTile.pos, Turn.currentSkill, true);

            // Debug.Log($"{GetType()} - �� �ߴ��� {(int)(AffectType)Enum.Parse(typeof(AffectType), Turn.currentSkill.affectType, true)}");

            board.ShowHighlightTile(tiles, 1);
            board.ShowAimingTile(tiles, (int)Turn.currentSkill.affectType);
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

    // ui Ȯ�� ��� 


    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        // �� �� ����     
    }
    private void TouchEnd(Vector2 screenPosition, float time)
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

    private IEnumerator AISkillTargeting()
    {
        if(Turn.currentSkill.isAOE)
        {
            tiles = searchMachine.SearchRange(aiController.aiPlan.targetPos, Turn.currentSkill, true);

            board.ShowHighlightTile(tiles, 1);
            board.ShowAimingTile(tiles, (int)Turn.currentSkill.affectType);
        }
        Turn.selectedTile.pos = aiController.aiPlan.targetPos;

        AddTarget();

        yield return new WaitForSeconds(1f);

        StateMachineController.instance.ChangeTo<PerformSkillState>();
    }


}
