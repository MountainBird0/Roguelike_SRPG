/******************************************************************************
* 사용자 입력 관리
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder((int)SEO.InputManager)]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    // 터치 관련
    private TouchControls touchControls;

    // 꾹 누를때 사용하고싶음
    public delegate void StartTouchEvent(Vector2 position, float time); // 위치, 시작시간
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time); // 위치, 시작시간
    public event EndTouchEvent OnEndTouch;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += context => StartTouch(context);
        touchControls.Touch.TouchPress.canceled += context => EndTouch(context);
    }

    /**********************************************************
    * 버튼 눌렀을 때
    ***********************************************************/
    private void StartTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("touch started" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());

        if (OnStartTouch != null)
        {
            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    /**********************************************************
    * 버튼 누른 손 떨어지면
    ***********************************************************/
    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("touch ended");

        if (OnEndTouch != null)
        {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }     
    }
}
