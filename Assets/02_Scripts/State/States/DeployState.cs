using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployState : State
{
    private Dictionary<Vector3Int, TileLogic> deployTiles;
    private DeployUIController controller;

    private string unitName;
    private Vector3Int oldCoords;
    private GameObject pickObj;

    private Coroutine coroutine;

    public override void Enter()
    {
        base.Enter();

        deployTiles = BattleMapManager.instance.deployTiles;
        controller = BattleMapUIManager.instance.deployUI;

        controller.EnableWindow();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        
        controller.DisableWindow();
        // deployTIles ���� �� �÷��а� mainTile�� ���� mgr���� ����?

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;

        BattleMapManager.instance.test(); ////////////////////////
        board.deploySpot.gameObject.SetActive(false);

        // ���ֵ� ������� unit ����Ʈ�� �ֱ�
        // ����Ʈ player������ ����
        // ���� �ӵ� ���� �ʿ伺
    }

    /**********************************************************
    * ��ġ ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) // �̹� ���� ������ �װ� ���
            {

                controller.EnableGuide();
                
                oldCoords = cellPosition;

                unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
                pickObj = deployTiles[cellPosition].content;

                coroutine = StartCoroutine(PickUnit());
            }
            else // ������ ���� ����
            {
                unitName = controller.unitName;
                if (unitName != null)
                {
                    GameObject ob = ObjectPoolManager.instance.Spawn(unitName);
                    ob.transform.position = cellPosition; 

                    deployTiles[cellPosition].content = ob;

                    controller.DisableButton(unitName);
                    controller.unitName = null;
                }
            }
        }
    }

    /**********************************************************
    * ��ġ ����
    ***********************************************************/
    private void TouchEnd(Vector2 screenPosition, float time)
    {
        controller.DisableGuide();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);

            coroutine = null;

            Vector3Int cellPosition = GetCellPosition(screenPosition);

            if (deployTiles.ContainsKey(cellPosition))
            {
                // �� �ڸ��� �̹� ���� ������
                if (deployTiles[cellPosition].content)
                {
                    var temp = deployTiles[oldCoords].content;
                    deployTiles[oldCoords].content = deployTiles[cellPosition].content;
                    deployTiles[cellPosition].content = temp;

                    deployTiles[cellPosition].content.transform.position = cellPosition;
                    deployTiles[oldCoords].content.transform.position = oldCoords;
                }
                else
                {
                    deployTiles[cellPosition].content = deployTiles[oldCoords].content;
                    deployTiles[cellPosition].content.transform.position = cellPosition;
                    deployTiles[oldCoords].content = null;
                }
            }
            // unit �����
            else
            {
                ObjectPoolManager.instance.Despawn(pickObj);
                deployTiles[oldCoords].content = null;

                controller.EnableButton(unitName);
                controller.unitName = null;
            }
        }
    }



    /**********************************************************
    * Ÿ�� ���� ���� ���
    ***********************************************************/
    private IEnumerator PickUnit()
    {
        while (true)
        {
            pickObj.transform.position = InputManager.instance.PrimaryPosition();
            yield return null;
        }
    }
}
