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
        Debug.Log($"{GetType()} - 실행");

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
        // deployTIles 삭제 밑 올려둔거 mainTile에 저장 mgr에서 할지?

        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;

        board.deploySpot.gameObject.SetActive(false);
    }

    /**********************************************************
    * 터치 시작
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = GetCellPosition(screenPosition);
        Debug.Log($"{GetType()} - 터치시작");
        if (deployTiles.ContainsKey(cellPosition))
        {
            if (deployTiles[cellPosition].content) // 이미 유닛 있으면 그거 들기
            {
                Debug.Log($"{GetType()} - 이미있음");

                controller.EnableGuide();
                
                oldCoords = cellPosition;

                unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
                pickObj = deployTiles[cellPosition].content;

                coroutine = StartCoroutine(PickUnit());
            }
            else // 없으면 새로 생성
            {
                Debug.Log($"{GetType()} - 없음");

                unitName = controller.unitName;
                if (unitName != null)
                {
                    GameObject go = ObjectPoolManager.instance.Spawn(unitName);
                    go.transform.position = cellPosition; 

                    deployTiles[cellPosition].content = go;

                    //foreach (var t in deployTiles)
                    //{
                    //    Debug.Log($"{GetType()} - 타일이름 위치 - {t}");
                    //}

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
        Debug.Log($"{GetType()} - 터치끝");

        controller.DisableGuide();

        if (coroutine != null)
        {
            Debug.Log($"{GetType()} - 리얼터치끝");

            StopCoroutine(coroutine);
            Debug.Log($"{GetType()} - 스탑");

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
        Debug.Log($"{GetType()} - 들기시작");

        while (true)
        {
            Debug.Log($"{GetType()} - 리얼들기");

            pickObj.transform.position = InputManager.instance.PrimaryPosition();
            yield return null;
        }
    }

    /**********************************************************
    * 마우스 위치 타일에 맞게 변환
    ***********************************************************/
    private Vector3Int GetCellPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;

        return board.deploySpot.WorldToCell(worldPosition);
    }
}
