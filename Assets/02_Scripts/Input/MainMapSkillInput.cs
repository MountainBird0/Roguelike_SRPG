/**********************************************************
* MainMap의 스킬창이 활성화 되었을 때의 Input
***********************************************************/
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

    [Header("ImagePool")]
    public IntKeyImagePool skillIconPool;

    private PointerEventData clickData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> clickResults = new();

    private Coroutine coroutine;

    private GameObject pastSlot;

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

    /**********************************************************
    * 스크린 터치 시작 / 종료
    ***********************************************************/
    private void TouchStart(Vector2 screenPosition, float time)
    {
        pastSlot = null;

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
        pastSlot = slot;
        
        var SlotInfo = pastSlot.GetComponent<SkillSlot>();

        if (SlotInfo.id.Equals(-1))
        {
            return;    
        }

        coroutine = StartCoroutine(Pickicon(SlotInfo.image.sprite));
    }

    /**********************************************************
    * 스킬 장착 칸에 스킬을 드랍 했을 때
    ***********************************************************/
    private void DropToEquipSlot(GameObject slot)
    {
        var prevSlotInfo = pastSlot.GetComponent<SkillSlot>();
        var equipSlotInfo = slot.GetComponent<SkillSlot>();

        if (prevSlotInfo.isEquipment)
        {
            SwapSkill(slot, pastSlot);
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
    private void SwapSkill(GameObject currentSlotOb, GameObject pastSlotOb)
    {
        var curSlot = currentSlotOb.GetComponent<SkillSlot>();
        var pastSlot = pastSlotOb.GetComponent<SkillSlot>();

        int tempId = curSlot.id;
        curSlot.id = pastSlot.id;
        pastSlot.id = tempId;

        Sprite tempImage = curSlot.image.sprite;
        curSlot.image.sprite = pastSlot.image.sprite;
        pastSlot.image.sprite = tempImage;
    }

    /**********************************************************
    * 빈 공간에 스킬을 드랍했을 때
    ***********************************************************/
    private void DropToEmpty()
    {
        var pastSlot = this.pastSlot.GetComponent<SkillSlot>();
        if (pastSlot.isEquipment)
        {
            controller.ChangeToTouchable(pastSlot.id);

            var slot = this.pastSlot.GetComponent<SkillSlot>();

            slot.id = -1;
            slot.image.sprite = MainMapUIManager.instance.defaultSprite;
        }
    }

    /**********************************************************
    * 터치한 스킬의 이미지 들기
    ***********************************************************/
    private IEnumerator Pickicon(Sprite image)
    {
        var skillSlot = pastSlot.GetComponent<SkillSlot>();
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
