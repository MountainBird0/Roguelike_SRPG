/**********************************************************
* �� unit�� ��� �� unit�� turn�� �����ϱ� ���� State
***********************************************************/
using UnityEngine;

public class TurnBeginState : State
{
    public override void Enter()
    {
        base.Enter();

        if(Turn.unit)
        {
            SelectUnit(Turn.selectedTile.pos);
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;
        }

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
        

        // �Ʊ� ���� ������ �� ���� �ӵ���� ����
        // �ӵ� ���� ���� ����
        //if(!BattleMapManager.instance.IsHuman())
        //{
        //    Turn.unit = BattleMapManager.instance.units[0];
        //    StateMachineController.instance.ChangeTo<ChooseActionState>();
        //}       
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (board.mainTiles.ContainsKey(cellPosition))
        {
            if (board.mainTiles[cellPosition].content)
            {
                SelectUnit(cellPosition);
            }
        }
    }
    private void TouchEnd(Vector2 screenPosition, float time)
    {

    }

    /**********************************************************
    * ���� �����ϱ�
    ***********************************************************/
    private void SelectUnit(Vector3Int cellPosition)
    {
        Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
        Turn.originTile = board.GetTile(cellPosition);
        Turn.currentTile = Turn.originTile;
        Turn.selectedTile = Turn.originTile;

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
