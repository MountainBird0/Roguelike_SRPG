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
    }

    public override void Exit()
    {
        base.Exit();
    }
}
