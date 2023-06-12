/******************************************************************************
* ����� �Է� ����
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder((int)SEO.InputManager)]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    // ��ġ ����
    private TouchControls touchControls;

    // �� ������ ����ϰ����
    public delegate void StartTouchEvent(Vector2 position, float time); // ��ġ, ���۽ð�
    public event StartTouchEvent OnStartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time); // ��ġ, ���۽ð�
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
    * ��ư ������ ��
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
    * ��ư ���� �� ��������
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
