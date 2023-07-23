using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    // 현재 선택한 아이콘 ui, index등
    // input에 접근
    // board에 접근


    protected StateMachineController machine;

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    // 타일 이동, ui 변경 등 함수들

}
