using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployState : State
{
    private Dictionary<Vector3Int, TileLogic> deployTiles = new();
    private DeployUIController uiController;

    private string unitName;
    private Vector3Int oldCoords;
    private GameObject pickObj;

    private Coroutine coroutine;

    public override void Enter()
    {
        base.Enter();

        board.SetTile(board.deployMap, deployTiles);
        uiController = BattleMapUIManager.instance.deployUI;

        uiController.EnableWindow();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        
        uiController.DisableWindow();
        board.deployMap.gameObject.SetActive(false);
        // deployTIles ���� �� �÷��а� mainTile�� ���� mgr���� ����?

        AddUnits();
        CopyContent();

        // ���ֵ� ������� unit ����Ʈ�� �ֱ�
        // ���� �ӵ� ���� �ʿ伺
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * ��ġ ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) 
            {
                // �̹� ���� ������ �װ� ���
                coroutine = StartCoroutine(PickUnit(cellPosition));
            }
            else 
            {
                // ������ ���� �����ؼ� ��ġ
                DeployNewUnit(cellPosition);
            }
        }
    }

    /**********************************************************
    * ��ġ ����
    ***********************************************************/
    private void TouchEnd(Vector2 screenPosition, float time)
    {
        uiController.DisableGuide();

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;

            Vector3Int cellPosition = GetCellPosition(screenPosition);

            if (deployTiles.ContainsKey(cellPosition))
            {
                if (deployTiles[cellPosition].content)
                {
                    ChangeUnit(cellPosition);
                }
                else
                {
                    deployTiles[cellPosition].content = deployTiles[oldCoords].content;
                    deployTiles[cellPosition].content.transform.position = cellPosition;
                    deployTiles[oldCoords].content = null;
                }
            }
            else
            {
                ObjectPoolManager.instance.Despawn(pickObj);
                deployTiles[oldCoords].content = null;

                uiController.EnableButton(unitName);
            }
        }
    }


    /**********************************************************
    * Ÿ�� ���� ���� ���
    ***********************************************************/
    private IEnumerator PickUnit(Vector3Int cellPosition)
    {
        uiController.EnableGuide();

        oldCoords = cellPosition;

        unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
        pickObj = deployTiles[cellPosition].content;

        while (true)
        {
            pickObj.transform.position = InputManager.instance.PrimaryPosition();
            yield return null;
        }
    }


    /**********************************************************
    * �� ���� ��ġ
    ***********************************************************/
    private void DeployNewUnit(Vector3Int cellPosition)
    {
        unitName = uiController.unitName;
        if (unitName != null)
        {
            GameObject ob = ObjectPoolManager.instance.Spawn(unitName);
            ob.transform.position = cellPosition;

            deployTiles[cellPosition].content = ob;

            uiController.DisableButton(unitName);
        }
    }
    

    /**********************************************************
    * Ÿ�� ���� ���ְ� ���� �ٲ�
    ***********************************************************/
    private void ChangeUnit(Vector3Int cellPosition)
    {
        var temp = deployTiles[oldCoords].content;
        deployTiles[oldCoords].content = deployTiles[cellPosition].content;
        deployTiles[cellPosition].content = temp;

        deployTiles[cellPosition].content.transform.position = cellPosition;
        deployTiles[oldCoords].content.transform.position = oldCoords;
    }


    /**********************************************************
    * units�� unit�߰�
    ***********************************************************/
    private void AddUnits()
    {
        foreach (var pair in deployTiles)
        {
            if(pair.Value.content != null)
            {
                var unit = pair.Value.content.GetComponent<Unit>();
                var tileLogic = pair.Value;
                BattleMapManager.instance.AddUnit(unit, tileLogic);
            }
        }
    }


    /**********************************************************
    * deployTilesŸ���� content�� mainTiles�� ����
    ***********************************************************/
    public void CopyContent()
    {
        foreach (var pair in deployTiles)
        {
            if (pair.Value.content != null)
            {
                board.mainTiles[pair.Key].content = pair.Value.content;
            }
        }
    }
}
