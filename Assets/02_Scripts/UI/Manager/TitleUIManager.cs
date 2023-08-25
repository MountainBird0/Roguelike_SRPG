/******************************************************************************
* Title狼 UI 包府, 积己 殿
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
