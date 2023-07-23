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

    private IEnumerator LoadSequence()
    {
        // �� ���� ��
        BattleMapManager.instance.MapLoad();
        yield return null;
        // deployState�� ����
        StateMachineController.instance.ChangeTo<DeployState>();
    }


}
