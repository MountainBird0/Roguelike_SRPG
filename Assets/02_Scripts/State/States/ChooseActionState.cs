using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseActionState : State
{
    // �����ϼ��ִ� ���� ǥ��
    // �����̱�
    // ������ ��ġ���� 
    // �����ӽ�ų ��� ����
    // �� �ѱ�� ��ư

    private Dictionary<Vector3Int, TileLogic> mainTiles;

    // test
    int range = 3;
    List<TileLogic> tiles;

    public override void Enter()
    {
        base.Enter();
        mainTiles = BattleMapManager.instance.mainTiles;

        ShowMoveableTile();

        //tile = BattleMapManager.instance.Search
        //    (BattleMapManager.instance.GetTile(Turn.unit.gameObject.transform.position), ValidateMovement);

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        BattleMapManager.instance.board.ClearHighTile(tiles);
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (mainTiles.ContainsKey(cellPosition))
        {
            if (mainTiles[cellPosition].content != null) // �� �����϶���
            {
                if (tiles != null)
                {
                    BattleMapManager.instance.board.ClearHighTile(tiles);
                }

                tiles = BattleMapManager.instance.Search(BattleMapManager.instance.GetTile(cellPosition), ValidateMovement);

                //foreach (var pair in tiles)
                //{
                //    Debug.Log($"{GetType()} main Key: " + pair + ", Value: " + pair.pos);
                //}

                BattleMapManager.instance.board.SetHighTile(tiles);

                //
            }
            else
            {
                StateMachineController.instance.ChangeTo<TurnBeginState>();
            }
        }
    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        
    }

    /**********************************************************
    * ������ �� �ִ� ���� ǥ��
    ***********************************************************/
    public void ShowMoveableTile()
    {

    }


    /**********************************************************
    * ������ �� �ִ� ���� �˻�
    ***********************************************************/
    // test
    public bool ValidateMovement(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;
        //to.distance = from.distance+from.moveCost;

        if (to.content != null || to.distance > range)
        {
            return false;
        }
        return true;
    }
}
