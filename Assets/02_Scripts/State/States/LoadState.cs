/**********************************************************
* Battle Scene�� �����ϰ� Map�� �����ϴ� State
***********************************************************/
using System.Collections;
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

    private IEnumerator LoadSequence() // �ڷ�ƾ ����?
    {
        // �� ���� ��
        BattleMapManager.instance.MapLoad(); // �� ����
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();
    }
}
