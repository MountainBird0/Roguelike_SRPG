/**********************************************************
* unit�� ��ġ�ϴ� State
***********************************************************/
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
        uiController = BattleMapUIManager.instance.deployUIController;

        uiController.EnableWindow();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        
        uiController.DisableWindow();
        board.deployMap.gameObject.SetActive(false);
        // deployTiles ���� �÷��а� mainTile�� ���� mgr���� ����?

        AddUnits();
        CopyContent();
        BattleMapManager.instance.ResetUnit();

        // �����̶� ���� ���õ� ��ų ������ �̸� �����ؼ� �־�α�?

        // ���� �ӵ� ���� �ʿ伺
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * ��ũ�� ��ġ ���� / ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content)
            { // �̹� ���� ������ �װ� ���            
                coroutine = StartCoroutine(PickUnit(cellPosition));
            }
            else
            { // ������ ���� �����ؼ� ��ġ             
                DeployNewUnit(cellPosition);
            }
        }
    }
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
                DropUnit(cellPosition);
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
    * �� ���� Ÿ�Ͽ� ��ġ
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
            uiController.DisableGuide();
        }
    }

    /**********************************************************
    * Ÿ�� ���� ���� ������
    ***********************************************************/
    private void DropUnit(Vector3Int cellPosition)
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
    * ��ġ�� ���� Manager�� �߰�
    ***********************************************************/
    private void AddUnits()
    {
        Unit unit;

        foreach (var kvp in deployTiles)
        {
            if(kvp.Value.content != null)
            {
                unit = kvp.Value.content.GetComponent<Unit>();
                unit.pos = kvp.Key;
                BattleMapManager.instance.UnitSetting(unit);
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
