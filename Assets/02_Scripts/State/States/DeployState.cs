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
        // deployTIles 삭제 밑 올려둔거 mainTile에 저장 mgr에서 할지?

        AddUnits();
        CopyContent();

        // 유닛들 순서대로 unit 리스트에 넣기
        // 몬스터 속도 정할 필요성
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    /**********************************************************
    * 터치 시작
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);

        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) 
            {
                // 이미 유닛 있으면 그거 들기
                coroutine = StartCoroutine(PickUnit(cellPosition));
            }
            else 
            {
                // 없으면 새로 생성해서 배치
                DeployNewUnit(cellPosition);
            }
        }
    }

    /**********************************************************
    * 터치 종료
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
    * 타일 위의 유닛 들기
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
    * 새 유닛 배치
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
    * units에 unit추가
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
}
