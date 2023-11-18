/******************************************************************************
* 로딩씬에서 로딩
*******************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    private AsyncOperation operation;

    public Image redBar;

    private void Start()
    {
        // 어디든 가도록 수정
        StartCoroutine(LoadCoroutine(1));
    }

    IEnumerator LoadCoroutine(int sceneNum)
    {
        operation = GlobalSceneManager.instance.GoScene(sceneNum);
        operation.allowSceneActivation = false;

        AddressableManager.instance.MakeImageAsync();

        float timer = 0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                redBar.fillAmount = Mathf.Lerp(operation.progress, 1f, timer);
                if(redBar.fillAmount >= operation.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                redBar.fillAmount = Mathf.Lerp(operation.progress, 1f, timer);
                if (redBar.fillAmount >= 0.99f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }

        gameObject.SetActive(false);
    }
}
