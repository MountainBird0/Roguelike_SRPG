/**********************************************************
* ���� ���� �Է� ����
***********************************************************/
using UnityEngine;

public class MainMapInput : MonoBehaviour
{
    private Ray ray;
    private RaycastHit2D hit;

    public MainMapInteraction interaction;

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += TouchStart;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= TouchStart;
    }

    /**********************************************************
    * ��ũ�� ��ġ ����
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
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

