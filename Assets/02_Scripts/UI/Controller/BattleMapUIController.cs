/******************************************************************************
* BattleScene의 UI 컨트롤
*******************************************************************************/
using UnityEngine;
using TMPro;

public class BattleMapUIController : MonoBehaviour
{

    public TextMeshProUGUI autoText;

    /**********************************************************
    * 테스트용 클리어 버튼 // 나중에 뒤로가기로
    ***********************************************************/
    public void ClickBtnClear()
    {
        GlobalSceneManager.instance.GoMainScene();
    }

    public void ClickBtnAuto()
    {
        if (BattleMapManager.instance.isBtnAuto)
        {
            autoText.text = "수동";
            BattleMapManager.instance.isBtnAuto = false;           
        }
        else
        {
            autoText.text = "자동";
            BattleMapManager.instance.isBtnAuto = true;
        }
    }
}
