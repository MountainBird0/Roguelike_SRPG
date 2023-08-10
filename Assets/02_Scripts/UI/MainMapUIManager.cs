/**********************************************************
* MainMapSceneÀÇ UI °ü¸®
***********************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMapUIManager : MonoBehaviour
{
    public TextMeshProUGUI seed;

    public ImagePool pool;

    public GameObject unitSlot;
    public Transform content;

    [Header("Controller")]
    public MainMapUIController controller;

    private void Start()
    {
        seed.text = DataManager.instance.gameInfo.seed.ToString();
        pool.MakeDictionarys();
        CreateUnitSlot();
    }

    public void CreateUnitSlot()
    {
        foreach (var kvp in DataManager.instance.currentUnitInfo)
        {
            var ob = Instantiate(unitSlot, content);
            ob.name = kvp.Key;
            ob.GetComponent<UnitSlot>().icon.sprite = pool.smallImages[kvp.Key];

            controller.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }

        controller.InitializeButtons();
    }   
}
