/**********************************************************
* Battle Scene에 진입하고 Map을 생성하는 State
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

    private IEnumerator LoadSequence() // 코루틴 이유?
    {
        // 맵 생성 등
        BattleMapManager.instance.MapLoad(); // 맵 생성
        yield return null;

        StateMachineController.instance.ChangeTo<DeployState>();
    }
}
