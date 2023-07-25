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
        // deployTIles 삭제 밑 올려둔거 mainTile에 저장 mgr에서 할지?

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;

        BattleMapManager.instance.test(); ////////////////////////
        board.deploySpot.gameObject.SetActive(false);

        // 유닛들 순서대로 unit 리스트에 넣기
        // 리스트 player먼저로 정렬
        // 몬스터 속도 정할 필요성
    }

    /**********************************************************
    * 터치 시작
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) // 이미 유닛 있으면 그거 들기
            {

                controller.EnableGuide();
                
                oldCoords = cellPosition;

                unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
                pickObj = deployTiles[cellPosition].content;

                coroutine = StartCoroutine(PickUnit());
            }
            else // 없으면 새로 생성
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
    * 터치 종료
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
                // 그 자리에 이미 유닛 있으면
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
            // unit 지우기
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
    * 타일 위의 유닛 들기
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
