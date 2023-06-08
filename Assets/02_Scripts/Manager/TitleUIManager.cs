/******************************************************************************
* TitleSceneÀÇ UI °ü¸®
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
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
