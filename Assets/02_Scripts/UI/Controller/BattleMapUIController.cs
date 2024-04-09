/******************************************************************************
* BattleScene�� UI ��Ʈ��
*******************************************************************************/
using UnityEngine;
using TMPro;

public class BattleMapUIController : MonoBehaviour
{

    public TextMeshProUGUI autoText;
    public TextMeshProUGUI turnCount;

    /**********************************************************
    * �׽�Ʈ�� Ŭ���� ��ư // ���߿� �ڷΰ����
    ***********************************************************/
    public void ClickBtnClear()
    {
        GlobalSceneManager.instance.GoMainScene();
    }

    public void SetTurnCount()
    {
        turnCount.text = Turn.turnCount.ToString() + " / 15";
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
