/**********************************************************
* �� unit�� ��� �� unit�� turn�� �����ϱ� ���� State
***********************************************************/
using UnityEngine;

public class TurnBeginState : State
{
    private TurnBeginUIController uiController = BattleMapUIManager.instance.turnBeginUIController;

    public override void Enter()
    {
        base.Enter();

        if(Turn.unit)
        {
            SelectUnit(Turn.selectedPos);
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;                   
        }
        // ������ �� �ִ� �Ʊ����� �ִ��� Ȯ��
        if(!Turn.isHumanTurn)
        {
            Debug.Log($"{GetType()} - AI��");
            foreach(var kvp in BattleMapManager.instance.AIUnits)
            {
                SelectUnit(kvp.Value.pos);
                break;
            }
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;
        }

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;    
    }

    public override void Exit()
    {
        base.Exit();

        uiController.DisableCanvas();

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
                var unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                if(BattleMapManager.instance.HumanUnits.ContainsKey(unit.unitNum))
                {
                    SelectUnit(cellPosition);
                }
                else
                {                   
                    uiController.ShowStatWindow(unit); // ����â �����ֱ�
                }
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

        Turn.originPos = cellPosition;
        Turn.currentPos = Turn.originPos;
        Turn.selectedPos = Turn.originPos;

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
