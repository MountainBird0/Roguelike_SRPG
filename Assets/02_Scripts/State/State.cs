using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // 현재 선택한 아이콘 ui, index등
    // input에 접근
    // board에 접근
    protected Board board;
    protected CameraController cameraController;
    //protected StateMachineController machine { get { return StateMachineController.instance; } };

    public void OnDisable()
    {
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    public virtual void Enter()
    {
        Debug.Log($"{GetType()} - 실행");
        if (board == null)
        {          
            board = BattleMapManager.instance.board; // 보드넣는거 awake에 넣을까
        }
        if(cameraController == null)
        {
            cameraController = BattleMapManager.instance.cameraController;
        }
    }

    public virtual void Exit()
    {
        Debug.Log($"{GetType()} - 종료");
    }

    public virtual void TouchStart(Vector2 screenPosition, float time)
    {

    }
    public virtual void TouchEnd(Vector2 screenPosition, float time)
    {

    }


    // 타일 이동, ui 변경 등 함수들

    /**********************************************************
    * 마우스 위치 타일에 맞게 변환
    ***********************************************************/
    protected Vector3Int GetCellPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;

        return board.deployMap.WorldToCell(worldPosition);
    }

    protected void SetCameraPos()
    {
        var pos = Turn.unit.pos;

        cameraController.cameraPos.position = new Vector3(pos.x, pos.y, -1);
    }

}
