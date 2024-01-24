using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDefeatState : State
{
    private StageDefeatUIController uiController = BattleMapUIManager.instance.stageDefeatUIController;


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
