/**********************************************************
* SkillSelectionState�� UI ��Ʈ��
***********************************************************/
using UnityEngine;

public class SkillSelectionUIController : MonoBehaviour
{
    public Canvas skillSelectionCanvas;


    /**********************************************************
    * ��ų ��� ��ư
    ***********************************************************/
    public void ClickBtnCancel()
    {
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(true);
    }
    public void DisableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(false);
    }
}
