/******************************************************************************
* BattleScene�� UI ��Ʈ��
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapUIController : MonoBehaviour
{
    /**********************************************************
    * �׽�Ʈ�� Ŭ���� ��ư // ���߿� �ڷΰ����
    ***********************************************************/
    public void ClickBtnClear()
    {
        GlobalSceneManager.instance.GoMainScene();
    }
}
