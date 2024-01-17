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
        if(Turn.isHumanTurn)
        {
            autoText.text = "�ڵ�";
        }
        else
        {
            autoText.text = "����";
        }
    }
}
