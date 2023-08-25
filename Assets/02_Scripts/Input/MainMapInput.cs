/**********************************************************
* 메인 맵의 입력 관리
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
    * 스크린 터치 시작
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        Debug.Log($"{GetType()} - 터치함");
        ray = Camera.main.ScreenPointToRay(screenPosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {            
            Debug.Log($"{GetType()} - 터치한거 {hit.transform.gameObject.name}");
            if (hit.transform.gameObject.CompareTag("Icon"))
            {
                interaction.GetIcon(hit.transform.gameObject);
            }
        }
    }
}

