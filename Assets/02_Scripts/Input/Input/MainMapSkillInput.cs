using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainMapSkillInput : MonoBehaviour
{
    public MainMapUIManager manager;
    public MainMapUIController controller;

    public GameObject moveIcon;

    public GameObject unitCanvas;
    public GraphicRaycaster raycaster;

    PointerEventData clickData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> clickResults = new();

    private Coroutine coroutine;
    private Sprite curSprite;

    private string skillName;
    private GameObject preOb;

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += TouchStart;
        InputManager.instance.OnEndTouch += TouchEnd;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= TouchStart;
        InputManager.instance.OnEndTouch -= TouchEnd;
    }

    private void TouchStart(Vector2 screenPosition, float time)
    {
        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);
        Debug.Log($"{GetType()} - {clickResults[0]}");

        var icon = clickResults[0].gameObject;
        // 누른게 스킬이면
        if (clickResults[0].gameObject.CompareTag("SkillUI"))
        {
            preOb = clickResults[1].gameObject; // 이전 슬롯
            skillName = icon.name;
            Debug.Log($"{GetType()} - 스킬이름 {skillName}");
            controller.ClickSkill(int.Parse(skillName));
            coroutine = StartCoroutine(PickUnit(icon.GetComponent<Image>().sprite));
        }
        else if(clickResults[0].gameObject.CompareTag("SkillSlot"))
        {

        }

        //foreach (RaycastResult result in clickResults)
        //{
        //    GameObject ui_element = result.gameObject;

        //    Debug.Log(ui_element.name);
        //}
        

        

    }

    private void TouchEnd(Vector2 screenPosition, float time)
    {
        moveIcon.SetActive(false);
        StopCoroutine(coroutine);

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);
        Debug.Log($"{GetType()} - {clickResults[2]}");

        var icon = clickResults[0].gameObject;
        // 놓은곳이 스킬장착
        if (clickResults[2].gameObject.name == "Panel_SkillSet")
        {
            icon.GetComponent<Image>().sprite = curSprite;
            icon.name = skillName;
            preOb.GetComponent<SkillSlot>().check.SetActive(true);
        }
        // 놓은곳이 스킬목록
        else
        {

        }
        // 놓은곳이 밖


    }


    private IEnumerator PickUnit(Sprite image)
    {
        yield return new WaitForSeconds(0.1f);
        moveIcon.SetActive(true);
        moveIcon.GetComponent<Image>().sprite = image;
        curSprite = image;
        while (true)
        {
            moveIcon.transform.position = InputManager.instance.UITouchPosition();
            // moveIcon.transform.position = Input.mousePosition;
            yield return null;
        }
    }


}
