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
        InputManager.instance.OnStartTouch += ScreenTouch;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= ScreenTouch;
    }

    public void ScreenTouch(Vector2 screenPosition, float time)
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
                    node.iconState = IconState.VISITED;

                    Debug.Log($"{GetType()} - 찾기성공 : {node.icon}");
                    GlobalSceneManager.instance.GoBattleScene();
                }
            }
        }
    }
}
