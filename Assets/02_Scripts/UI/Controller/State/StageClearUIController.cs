using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearUIController : MonoBehaviour
{
    public Canvas clearCanvas;



    public void EnableCanvas()
    {
        clearCanvas.gameObject.SetActive(true);
        SetClearWindow();
    }

    /**********************************************************
    * Ŭ���������� ����
    ***********************************************************/
    private void SetClearWindow()
    {
        BattleMapUIManager.instance.CreateResultSlot();
    }

    /**********************************************************
    * ����ȭ�� �̵� ��ư
    ***********************************************************/
    public void ClickBtnEnd()
    {
        GlobalSceneManager.instance.GoMainScene();
    }



}
