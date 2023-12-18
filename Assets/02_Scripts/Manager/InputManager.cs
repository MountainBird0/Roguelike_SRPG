/******************************************************************************
* 사용자 입력 관리
*******************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DefaultExecutionOrder((int)SEO.InputManager)]
public class InputManager : MonoBehaviour
{
    private TouchControls touchControls;

    public delegate void StartTouchEvent(Vector2 position, float time); // 위치, 시작시간
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time); // 위치, 시작시간
    public event EndTouchEvent OnEndTouch;

    public PointerEventData clickData = new PointerEventData(EventSystem.current);
    public List<RaycastResult> clickResults = new();

    public static InputManager instance;
    private void Awake()
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
        Debug.Log("touch started" + touchControls.Touch.TouchPosition.ReadValue<Vector2>());

        if (OnStartTouch != null)
        {
            clickResults.Clear();
            clickData.position = touchControls.Touch.TouchPosition.ReadValue<Vector2>();

            OnStartTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
        }
    }

    /**********************************************************
    * 버튼 누른 손 떨어지면
    ***********************************************************/
    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(touchControls.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
        }     
    }

    /**********************************************************
    * 현재 마우스 위치 변환 - UI용
    ***********************************************************/
    public Vector2 UITouchPosition()
    {
        return touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    }

    /**********************************************************
    * 현재 마우스 위치 변환 - 일반
    ***********************************************************/
    public Vector2 PrimaryPosition()
    {
        return Camera.main.ScreenToWorldPoint(touchControls.Touch.TouchPosition.ReadValue<Vector2>());
    }

    /**********************************************************
    * 누른위치 UI들 받아오기
    ***********************************************************/
    public void RaycastUI(GraphicRaycaster rayCaster)
    {
        rayCaster.Raycast(clickData, clickResults);
    }
}
