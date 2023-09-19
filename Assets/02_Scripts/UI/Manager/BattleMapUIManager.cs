/**********************************************************
* BattleMap�� UI ����, ���� ��
***********************************************************/
using UnityEngine;
using UnityEngine.UI;

public class BattleMapUIManager : MonoBehaviour
{
    public static BattleMapUIManager instance;

    public GameObject deploySlot;
    public Transform deployWindow;

    [Header("Controller")] // getcomponent�� ����
    public DeployUIController deployUIController;
    public TurnBeginUIController turnBeginUIController;
    public ChooseActionUIController ChooseActionUIController;
    public SkillSelectionUIController skillSelectionUIController;
    public ArrowSelectionUIController arrowSelectionUIController;
    public SkillTargetUIController skillTargetUIController;

    [Header("ImagePool")]
    public StringKeyImagePool unitSmallPool;
    public StringKeyImagePool unitBigPool;
    public IntKeyImagePool skillIconPool;

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

    /**********************************************************
    * ��ġ������ ���� ����
    ***********************************************************/
    public void CreateDeploySlot()
    {
        foreach(var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(deploySlot, deployWindow);

            var set = ob.GetComponent<DeploySlot>();
            set.image.sprite = unitSmallPool.images[kvp.Key];
            set.className.text = kvp.Key;
            set.level.text = kvp.Value.Level.ToString();
            set.hp.text = kvp.Value.HP.ToString();

            deployUIController.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }
    }
}
