using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : State
{
    public override void Enter()
    {
        base.Enter();

        StartCoroutine(LoadSequence());
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator LoadSequence()
    {
        // ¸Ê »ý¼º µî
        BattleMapManager.instance.MapLoad();
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();
    }


}
