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
            SelectUnit(Turn.selectedTile.pos);
            StateMachineController.instance.ChangeTo<ChooseActionState>();
            return;                   
        }
        // ������ �� �ִ� �Ʊ����� �ִ��� Ȯ��
        if(!Turn.isHumanTurn)
        {
            Debug.Log($"{GetType()} - AI��");
            foreach(var kvp in BattleMapManager.instance.AIUnits)
            {
                SelectUnit(kvp.Value.currentPos);
                break;
            }
            StateMachineController.instance.ChangeTo<ChooseActionState>();
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

        Turn.originTile = board.GetTile(cellPosition);
        Turn.currentTile = Turn.originTile;
        Turn.selectedTile = Turn.originTile;

        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }
}
