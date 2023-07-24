using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadState : State
{
    public override void Enter()
    {
        //base.Enter();
        Debug.Log($"{GetType()} - ����");

        BattleMapManager.instance.MapLoad();
        StateMachineController.instance.ChangeTo<DeployState>();

        //StartCoroutine(LoadSequence());


    }

    private IEnumerator LoadSequence()
    {
        // �� ���� ��
        BattleMapManager.instance.MapLoad();
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();

    }


}
