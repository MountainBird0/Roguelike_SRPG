/**********************************************************
* unit을 배치하는 State
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployState : State
{
    private Dictionary<Vector3Int, TileLogic> deployTiles = new();
    private DeployUIController uiController;
    private TurnBeginUIController unitInfoController = BattleMapUIManager.instance.turnBeginUIController;

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
        unitInfoController.DisableCanvas();

        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    public override void Exit()
    {
        base.Exit();
        
        uiController.DisableWindow();
        board.deployMap.gameObject.SetActive(false);
        // deployTiles 삭제 올려둔거 mainTile에 저장 mgr에서 할지?

        AddUnits();
        CopyContent();
        BattleMapManager.instance.ResetUnit();

        // 유닛이랑 몬스터 선택된 스킬 프리팹 미리 생성해서 넣어두기?

        // 몬스터 속도 정할 필요성
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
        Debug.Log($"{GetType()} - 터치한거 날라감");
    }

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    public override void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content)
            { // 이미 유닛 있으면 그거 들기            
                coroutine = StartCoroutine(PickUnit(cellPosition));
            }
            else
            { // 없으면 새로 생성해서 배치             
                DeployNewUnit(cellPosition);
                uiController.EnableFinishButton();
            }
        }
        else if(board.mainTiles.ContainsKey(cellPosition))
        {
            if (board.mainTiles[cellPosition].content)
            {
                var unit = board.mainTiles[cellPosition].content.GetComponent<Unit>();
                unitInfoController.ShowStatWindow(unit);
            }
            else
            {
                unitInfoController.DisableCanvas();
            }
        }
        else
        {
            unitInfoController.DisableCanvas();
        }
    }
    public override void TouchEnd(Vector2 screenPosition, float time)
    {
        uiController.DisableGuide();

        if (coroutine != null)
        {
            InputManager.instance.isCameraLock = false;

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

                if(!IsUnitDeploy())
                {
                    uiController.DisableFinishButton();
                }

                uiController.EnableButton(unitName);
            }
        }
    }

    /**********************************************************
    * 타일 위의 유닛 들기
    ***********************************************************/
    private IEnumerator PickUnit(Vector3Int cellPosition)
    {
        InputManager.instance.isCameraLock = true;
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
    * 새 유닛 타일에 배치
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
    * 타일 위에 유닛 떨구기
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
    * 타일 위의 유닛과 서로 바꿈
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
    * 배치한 유닛 Manager에 추가
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
    * deployTiles타일의 content를 mainTiles로 복사
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

    /**********************************************************
    * 현재 타일 위에 유닛 하나도 없는지 확인
    ***********************************************************/
    public bool IsUnitDeploy()
    {
        foreach (var kvp in deployTiles)
        {
            if (kvp.Value.content != null)
            {
                return true;
            }
        }

        return false;
    }
}
