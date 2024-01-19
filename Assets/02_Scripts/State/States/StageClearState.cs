using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearState : State
{
    private StageClearUIController uiController = BattleMapUIManager.instance.stageClearUIController;

    public override void Enter()
    {
        base.Enter();

        uiController.EnableCanvas();

        Debug.Log($"{GetType()} - 클리어스테이지 옴");
    }

    public override void Exit()
    {
        base.Exit();
    }
}
