/**********************************************************
* 메인 맵의 입력 관리
***********************************************************/
using UnityEngine;

public class MainMapInput : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

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
        ray = Camera.main.ScreenPointToRay(screenPosition);

        if(Physics.Raycast(ray, out hit))
        {            
            Debug.Log($"{GetType()} - 터치한거 {hit.transform.gameObject.name}");
            if (hit.transform.gameObject.CompareTag("Icon"))
            {
                interaction.GetIcon(hit.transform.gameObject);
            }
        }
    }
}
