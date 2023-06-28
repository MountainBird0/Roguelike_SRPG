/******************************************************************************
* TitleSceneÀÇ UI °ü¸®
*******************************************************************************/
using UnityEngine;

[DefaultExecutionOrder((int)SEO.TitleUIManager)]
public class TitleUIManager : MonoBehaviour
{
    public GameObject btnContinue;

    private void Start()
    {
        if(GameManager.instance.hasSaveData)
        {
            btnContinue.SetActive(true);
        }
    }
}
