using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapUIManager : MonoBehaviour
{
    public static BattleMapUIManager instance;

    public GameObject deploySlot;
    public Transform deployWindow;

    [Header("Controller")]
    public DeployUIController deployUI;
    public ChooseActionUIController ActionUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
    }

    public void CreateDeploySlot()
    {
        foreach(var kvp in DataManager.instance.currentUnitInfo)
        {
            var ob = Instantiate(deploySlot, deployWindow);
            ob.name = kvp.Key;
            // ¼öÁ¤..
            var temp = ObjectPoolManager.instance.Spawn(kvp.Key);
            temp.transform.position = new Vector3(400, 400, 400);

            var set = ob.GetComponent<DeploySlot>();

            set.bigIcon.sprite = temp.GetComponent<Unit>().smallIcon;
            ObjectPoolManager.instance.Despawn(temp);

            set.className.text = kvp.Key;
            set.level.text = kvp.Value.Level.ToString();
            set.hp.text = kvp.Value.HP.ToString();

            deployUI.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }
    }
}
