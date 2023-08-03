using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionState : State
{
    private ChooseActionUIController uiController = BattleMapUIManager.instance.ActionUI;
    
    // �����ϼ��ִ� ���� ǥ��
    // �����̱�
    // ������ ��ġ���� 
    // �����ӽ�ų ��� ����
    // �� �ѱ�� ��ư

    private List<TileLogic> tiles;

    public override void Enter()
    {
        base.Enter();

        uiController.EnableCanvas();

        if(!Turn.hasMoved)
        {
            ShowMoveableTile();
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

    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        // 1. �������� �ƹ����� �ƴҶ�
        // -> �׳� TurnBeginState�� 

        // 2. �������� �ٸ� unit �� ��
        // -> Turn.unit �װɷ� �ٲٰ� TurnBeginState��

        // 3. �������� �ڱ��ڽ� �� ��
        // -> �������� ǥ��??

        // 4. �������� ������ �� �ִ� ��Ҷ��
        // -> MoveSequenceState��

        // 5. ��ų ������ ������
        // -> ���� unit�� position(selectedTile)�� content�� unit �����ؾ��ҵ�?

        // �̵������� Ÿ���� ��ġ�ߴٸ�
        if (board.highlightTiles.ContainsKey(cellPosition))
        {
            Turn.selectedTile = new TileLogic(cellPosition);
            StateMachineController.instance.ChangeTo<MoveSequenceState>();
        }
        // �̵� �Ұ����� ���� ��ġ�ߴٸ�
        else if (board.mainTiles.ContainsKey(cellPosition))
        {
            board.ClearHighTile(tiles);
            Turn.hasMoved = false;
            Turn.unit.gameObject.transform.position = Turn.prevTile.pos;

            // �������� �̹� ������ �ִٸ�
            if (board.mainTiles[cellPosition].content) // �� �����϶��� �߰�
            {                
                Debug.Log($"{GetType()} - �ٸ���������");
                Turn.prevTile = board.GetTile(cellPosition);
                Turn.unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();

                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
            else
            {
                Debug.Log($"{GetType()} - �����");
                Turn.unit = null;
                Turn.prevTile = null; // ���ص��ǳ�
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
        //else
        //{
        //    // board.ClearHighTile(tiles);
        //    Turn.unit = null;
        //    StateMachineController.instance.ChangeTo<TurnBeginState>();
        //}
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        
    }

    /**********************************************************
    * ������ �� �ִ� ���� ǥ��
    ***********************************************************/
    public void ShowMoveableTile()
    {
        tiles = board.Search(board.GetTile(Turn.prevTile.pos), ValidateMovement);
        board.ShowHighTile(tiles);
    }


    /**********************************************************
    * ������ �� �ִ� ���� �˻�
    ***********************************************************/
    // test �̰͵��� ��� �־���ұ�
    public bool ValidateMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        int range = Turn.unit.stats.MOV;

        if (to.content != null || to.distance > range)
        {
            return false;
        }
        return true;
    }
}
