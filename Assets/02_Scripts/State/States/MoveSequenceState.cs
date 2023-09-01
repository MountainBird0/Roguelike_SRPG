/**********************************************************
* unit�� �������� �̷������ State
***********************************************************/
public class MoveSequenceState : State
{
    public override void Enter()
    {
        base.Enter();
        MoveUnit();
    }

    public override void Exit()
    {
        base.Exit();

    }

    private void MoveUnit()
    {
        Turn.unit.gameObject.transform.position = Turn.selectedTile.pos;
        Turn.isMoving = true;
        // �����̴� ani �߰�

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
