/**********************************************************
* ���� ���� �Է� ����
***********************************************************/
using UnityEngine;

public class MainMapInput : MonoBehaviour
{
    Ray ray;
    RaycastHit2D hit;

    public MainMapInteraction interaction;

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += ScreenTouch;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= ScreenTouch;
    }

    private void ScreenTouch(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - ��ġ��");
        ray = Camera.main.ScreenPointToRay(screenPosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {            
            Debug.Log($"{GetType()} - ��ġ�Ѱ� {hit.transform.gameObject.name}");
            if (hit.transform.gameObject.CompareTag("Icon"))
            {
                interaction.GetIcon(hit.transform.gameObject);
            }
        }
    }
}

