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
        foreach(var kvp in DataManager.instance.defaultUnitStats)
        {
            var ob = Instantiate(deploySlot, deployWindow);
            ob.name = kvp.Key;
            // ¼öÁ¤..
            var temp = ObjectPoolManager.instance.Spawn(kvp.Key);
            temp.transform.position = new Vector3(400, 400, 400);
            ob.GetComponent<DeploySlot>().bigIcon.sprite = temp.GetComponent<Unit>().bigIcon;
            ObjectPoolManager.instance.Despawn(temp);

            ob.GetComponent<DeploySlot>().className.text = kvp.Key;
            ob.GetComponent<DeploySlot>().level.text = kvp.Value.Level.ToString();
            ob.GetComponent<DeploySlot>().hp.text = kvp.Value.HP.ToString();

            deployUI.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }
    }
}
