/**********************************************************
* 메인 맵 플레이어 인풋 관리
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
            //if(hit.transform.gameObject.CompareTag("Monster"))
            //{
            //    Debug.Log($"{GetType()} - 몬스터 버튼 누름");
            //}

            foreach(var node in DataManager.instance.nodes)
            {
                if (node.icon == hit.transform.gameObject)
                {
                    Debug.Log($"{GetType()} - 찾기성공 : {node.icon}");
                    foreach(var c in node.connectedNodes)
                    {
                        Debug.Log($"{GetType()} - {c.icon}");
                    }
                }
            }
        }
    }
}
