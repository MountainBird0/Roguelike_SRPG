using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployState : State
{
    private Board board;
    private Dictionary<Vector3Int, TileLogic> deployTiles;
    private DeployUIController controller;

    private string unitName;
    private Vector3Int oldCoords;
    private GameObject pickObj;

    private Coroutine coroutine;

    public override void Enter()
    {
        Debug.Log($"{GetType()} - ����");

        //base.Enter();

        board = BattleMapManager.instance.board;
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

        board.deploySpot.gameObject.SetActive(false);
    }

    /**********************************************************
    * ��ġ ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        Debug.Log($"{GetType()} - ��ġ����");
        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) // �̹� ���� ������ �װ� ���
            {
                Debug.Log($"{GetType()} - �̹�����");

                controller.EnableGuide();
                
                oldCoords = cellPosition;

                unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
                pickObj = deployTiles[cellPosition].content;

                coroutine = StartCoroutine(PickUnit());
            }
            else // ������ ���� ����
            {
                Debug.Log($"{GetType()} - ����");

                unitName = controller.unitName;
                if (unitName != null)
                {
                    GameObject go = ObjectPoolManager.instance.Spawn(unitName);
                    go.transform.position = cellPosition; 

                    deployTiles[cellPosition].content = go;

                    //foreach (var t in deployTiles)
                    //{
                    //    Debug.Log($"{GetType()} - Ÿ���̸� ��ġ - {t}");
                    //}

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
        Debug.Log($"{GetType()} - ��ġ��");

        controller.DisableGuide();

        if (coroutine != null)
        {
            Debug.Log($"{GetType()} - ������ġ��");

            StopCoroutine(coroutine);
            Debug.Log($"{GetType()} - ��ž");

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
        Debug.Log($"{GetType()} - ������");

        while (true)
        {
            Debug.Log($"{GetType()} - ������");

            pickObj.transform.position = InputManager.instance.PrimaryPosition();
            yield return null;
        }
    }

    /**********************************************************
    * ���콺 ��ġ Ÿ�Ͽ� �°� ��ȯ
    ***********************************************************/
    private Vector3Int GetCellPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;

        return board.deploySpot.WorldToCell(worldPosition);
    }
}
