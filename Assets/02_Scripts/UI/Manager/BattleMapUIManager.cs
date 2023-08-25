/**********************************************************
* BattleMap의 UI 관리, 생성 등
***********************************************************/
using UnityEngine;
using UnityEngine.UI;

public class BattleMapUIManager : MonoBehaviour
{
    public static BattleMapUIManager instance;

    public GameObject deploySlot;
    public Transform deployWindow;

    [Header("Controller")] // getcomponent로 변경
    public DeployUIController deployUIController;
    public ChooseActionUIController ChooseActionUIController;
    public SkillSelectionUIController skillSelectionUIController;

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

    private void Start()
    {
        ChooseActionUIController.imagePool.MakeDictionarys();
    }

    /**********************************************************
    * 배치가능한 유닛 생성
    ***********************************************************/
    public void CreateDeploySlot()
    {
        foreach(var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(deploySlot, deployWindow);
            ob.name = kvp.Key;
            // 수정..
            var temp = ObjectPoolManager.instance.Spawn(kvp.Key);
            temp.transform.position = new Vector3(400, 400, 400);

            var set = ob.GetComponent<DeploySlot>();

            set.bigIcon.sprite = temp.GetComponent<Unit>().smallIcon;
            ObjectPoolManager.instance.Despawn(temp);

            set.className.text = kvp.Key;
            set.level.text = kvp.Value.Level.ToString();
            set.hp.text = kvp.Value.HP.ToString();

            deployUIController.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }
    }
}
