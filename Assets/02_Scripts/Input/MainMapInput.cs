/**********************************************************
* ���� �� �÷��̾� ��ǲ ����
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMapInput : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += Move;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= Move;
    }

    public void Move(Vector2 screenPosition, float time)
    {
        ray = Camera.main.ScreenPointToRay(screenPosition);

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log($"{GetType()} - {hit.transform.gameObject.name}");
            if(hit.transform.gameObject.CompareTag("Monster"))
            {
                Debug.Log($"{GetType()} - ���� ��ư ����");
            }
        }
    }
}
