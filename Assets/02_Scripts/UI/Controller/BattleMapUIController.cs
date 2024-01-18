/******************************************************************************
* BattleScene�� UI ��Ʈ��
*******************************************************************************/
using UnityEngine;
using TMPro;

public class BattleMapUIController : MonoBehaviour
{

    public TextMeshProUGUI autoText;

    /**********************************************************
    * �׽�Ʈ�� Ŭ���� ��ư // ���߿� �ڷΰ����
    ***********************************************************/
    public void ClickBtnClear()
    {
        GlobalSceneManager.instance.GoMainScene();
    }

    public void ClickBtnAuto()
    {
        if (BattleMapManager.instance.isBtnAuto)
        {
            autoText.text = "����";
            BattleMapManager.instance.isBtnAuto = false;           
        }
        else
        {
            autoText.text = "�ڵ�";
            BattleMapManager.instance.isBtnAuto = true;
        }
    }
}
