/**********************************************************
* MainMapSceneÀÇ UI °ü¸®
***********************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMapUIManager : MonoBehaviour
{
    public TextMeshProUGUI seed;

    private void Start()
    {
        seed.text = DataManager.instance.gameInfo.seed.ToString();
    }
}
