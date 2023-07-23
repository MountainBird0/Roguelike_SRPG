/**********************************************************
* 게임시작전 유닛을 타일에 배치할 때 사용
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// 순서 신경쓰기
public class DeployUnit : MonoBehaviour
{
    public DeployUIController controller;
    
    private Board board;

    public Dictionary<Vector3Int, TileLogic> deployTiles;

    private Coroutine coroutine;

    private Vector3Int preUnitPosition;

    private string unitName;

    private GameObject pickObj;

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += TouchForDeploy;
        InputManager.instance.OnEndTouch += DeployEnd;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= TouchForDeploy;
        InputManager.instance.OnEndTouch -= DeployEnd;

    }

    private void Start()
    {
        board = BattleMapManager.instance.board;
        deployTiles = BattleMapManager.instance.deployTiles;
    }

    private void TouchForDeploy(Vector2 screenPosition, float time)
    {
        Vector3Int cellPosition = SetCellPosition(screenPosition);

        if (deployTiles.ContainsKey(cellPosition))
        {
            // 눌렀을때 이미 unit있으면 그거 들기
            if (deployTiles[cellPosition].content)
            {
                preUnitPosition = cellPosition;
                
                controller.EnableGuide();
                unitName = deployTiles[cellPosition].content.GetComponent<Unit>().unitName;
                pickObj = deployTiles[cellPosition].content;

                coroutine = StartCoroutine(PickUnit());  
            }
            // 없으면 새로 생성
            else
            {
                unitName = controller.unitName;
                if(unitName != null)
                {
                    GameObject go = ObjectPoolManager.instance.Spawn(unitName);
                    go.transform.position = cellPosition;

                    deployTiles[cellPosition].content = go;

                    controller.DisableButton(unitName);

                    controller.unitName = null;
                }
            }
        }
    }


    private void DeployEnd(Vector2 screenPosition, float time)
    {
        controller.DisableGuide();

        if (coroutine != null)
        {

            StopCoroutine(coroutine);
            coroutine = null;
        
            Vector3Int cellPosition = SetCellPosition(screenPosition);

            if (deployTiles.ContainsKey(cellPosition))
            {
                // 그 자리에 이미 유닛 있으면
                if (deployTiles[cellPosition].content)
                {                
                    var temp = deployTiles[preUnitPosition].content;
                    deployTiles[preUnitPosition].content = deployTiles[cellPosition].content;
                    deployTiles[cellPosition].content = temp;

                    deployTiles[cellPosition].content.transform.position = cellPosition;
                    deployTiles[preUnitPosition].content.transform.position = preUnitPosition;
                }
                else
                {   
                    deployTiles[cellPosition].content = deployTiles[preUnitPosition].content;
                    deployTiles[cellPosition].content.transform.position = cellPosition;
                    deployTiles[preUnitPosition].content = null;
                }
            }
            // unit 지우기
            else
            {
                ObjectPoolManager.instance.Despawn(pickObj);
                deployTiles[preUnitPosition].content = null;

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
        while(true)
        {
            pickObj.transform.position = InputManager.instance.PrimaryPosition();
            yield return null;
        }
    }

    /**********************************************************
    * 마우스 위치 변환
    ***********************************************************/
    private Vector3Int SetCellPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;

        return board.deploySpot.WorldToCell(worldPosition);
    }

}
