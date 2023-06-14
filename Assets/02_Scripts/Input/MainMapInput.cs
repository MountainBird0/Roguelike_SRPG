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

    public MainMapInteraction interaction;

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
            if (hit.transform.gameObject.CompareTag("Icon"))
            {
                Debug.Log($"{GetType()} - ���� ��ư ����");
                IconNode node = DataManager.instance.nodes.Find(node => node.icon == hit.transform.gameObject);
                interaction.ChangeState(node);
            }
        }
    }
}
