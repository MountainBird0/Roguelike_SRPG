using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MainMapSkillInput : MonoBehaviour
{
    public MainMapUIController controller;

    public GameObject moveIcon;

    public GameObject unitCanvas;
    public GraphicRaycaster raycaster;

    private PointerEventData clickData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> clickResults = new();

    private Coroutine coroutine;

    private GameObject prevSlot;

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
        prevSlot = null;

        clickResults.Clear();
        clickData.position = screenPosition;
        raycaster.Raycast(clickData, clickResults);

        //Debug.Log($"{GetType()} - 누른거 이름 {clickResults[0]}");

        var ob = clickResults[1].gameObject;
        if (ob.CompareTag("SkillSlot") || ob.CompareTag("EquipSlot"))
        {
            TouchSkillSlot(ob);
        }
    }


    private void TouchEnd(Vector2 screenPosition, float time)
    {      
        if(coroutine != null)
        {
            moveIcon.SetActive(false);

            StopCoroutine(coroutine);

            clickResults.Clear();
            clickData.position = screenPosition;
            raycaster.Raycast(clickData, clickResults);

            if(clickResults.Count > 2)
            {
                var ob = clickResults[1].gameObject;
                if (ob.CompareTag("EquipSlot"))
                {
                    DropToEquipSlot(ob);
                }
                else
                {
                    DropToEmpty();
                }
            }

            coroutine = null;
        }
    }


    /**********************************************************
    * 스킬을 터치 했을 때
    ***********************************************************/
    private void TouchSkillSlot(GameObject slot)
    {
        prevSlot = slot;
        var prevSlotInfo = prevSlot.GetComponent<SkillSlot>();

        coroutine = StartCoroutine(Pickicon(prevSlotInfo.image.sprite));
    }


    /**********************************************************
    * 스킬 장착 칸에 스킬을 드랍 했을 때
    ***********************************************************/
    private void DropToEquipSlot(GameObject slot)
    {
        var prevSlotInfo = prevSlot.GetComponent<SkillSlot>();
        var equipSlotInfo = slot.GetComponent<SkillSlot>();

        if (prevSlotInfo.isEquipment)
        {
            SwapSkill(slot, prevSlot);
        }
        else
        {
            prevSlotInfo.check.SetActive(true);

            if(equipSlotInfo.id != -1)
            {
                controller.ChangeToTouchable(equipSlotInfo.id);
            }
            
            equipSlotInfo.id = prevSlotInfo.id;
            equipSlotInfo.image.sprite = prevSlotInfo.image.sprite;
        }
    }
    private void SwapSkill(GameObject currentSlot, GameObject prevSlot)
    {
        int id = currentSlot.GetComponent<SkillSlot>().id;
        Sprite image = currentSlot.GetComponent<SkillSlot>().image.sprite;

        currentSlot.GetComponent<SkillSlot>().id = prevSlot.GetComponent<SkillSlot>().id;
        currentSlot.GetComponent<SkillSlot>().image.sprite = prevSlot.GetComponent<SkillSlot>().image.sprite;

        prevSlot.GetComponent<SkillSlot>().id = id;
        prevSlot.GetComponent<SkillSlot>().image.sprite = image;

    }


    /**********************************************************
    * 빈공간에 스킬을 드랍했을 때
    ***********************************************************/
    private void DropToEmpty()
    {
        var prevSlotInfo = prevSlot.GetComponent<SkillSlot>();
        if (prevSlotInfo.isEquipment)
        {
            controller.ChangeToTouchable(prevSlotInfo.id);

            var slot = prevSlot.GetComponent<SkillSlot>();

            slot.id = -1;
            slot.image.sprite = controller.pool.skillImages[slot.id];
        }
    }


    /**********************************************************
    * 터치한 이미지에 맞는 아이콘 들기
    ***********************************************************/
    private IEnumerator Pickicon(Sprite image)
    {
        var skillSlot = prevSlot.GetComponent<SkillSlot>();
        controller.ClickSkill(skillSlot.id);

        yield return new WaitForSeconds(0.1f);

        moveIcon.SetActive(true);
        moveIcon.GetComponent<Image>().sprite = image;

        while (true)
        {
            moveIcon.transform.position = InputManager.instance.UITouchPosition();
            yield return null;
        }
    }
}
