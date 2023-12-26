/**********************************************************
* SkillSelectionState�� UI ��Ʈ��
***********************************************************/
using DG.Tweening;
using UnityEngine;

public class SkillSelectionUIController : MonoBehaviour
{
    public Canvas skillSelectionCanvas;

    public GameObject ChooseList;

    /**********************************************************
    * ��ų ��� ��ư
    ***********************************************************/
    public void ClickBtnCancel()
    {
        if(Turn.skill.data.isDirectional)
        {
            StateMachineController.instance.ChangeTo<ArrowSelectionState>();
            return;
        }
        StateMachineController.instance.ChangeTo<ChooseActionState>();
    }

    /**********************************************************
    * ���� ĵ���� Ȱ��ȭ/��Ȱ��ȭ
    ***********************************************************/
    public void EnableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(true);
        ChooseList.transform.DORotate(new Vector3(0, 0, 150), 0.2f).From(true);
    }
    public void DisableCanvas()
    {
        skillSelectionCanvas.gameObject.SetActive(false);
    }
}
