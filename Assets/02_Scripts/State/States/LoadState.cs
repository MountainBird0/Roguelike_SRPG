using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : State
{
    public override void Enter()
    {
        //base.Enter();
        Debug.Log($"{GetType()} - 실행");

        StartCoroutine(LoadSequence());
    }

    private IEnumerator LoadSequence()
    {
        // 맵 생성 등
        BattleMapManager.instance.MapLoad();
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();
    }


}
