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

    private IEnumerator LoadSequence() // 内风凭 捞蜡?
    {
        // 甘 积己 殿
        BattleMapManager.instance.MapLoad(); // 甘 积己
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();
    }


}
