/**********************************************************
* BattleMap�� UI ����, ���� ��
***********************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleMapUIManager : MonoBehaviour
{
    public static BattleMapUIManager instance;

    [Header("DeploySlot")]
    public GameObject deploySlot;
    public Transform deployWindow;

    [Header("ResultSlot")]
    public GameObject resultSlot;
    public Transform resultWindow;

    [Header("Controller")]
    public DeployUIController deployUIController;
    public TurnBeginUIController turnBeginUIController;
    public ChooseActionUIController chooseActionUIController;
    public SkillSelectionUIController skillSelectionUIController;
    public ArrowSelectionUIController arrowSelectionUIController;
    public SkillTargetUIController skillTargetUIController;
    public StageClearUIController stageClearUIController;

    [Header("ImagePool")]
    public StringKeyImagePool unitSmallPool;
    public StringKeyImagePool unitBigPool;
    public IntKeyImagePool skillIconPool;

    [Header("DefaultImage")]
    public Sprite defaultSprite;


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

        deployUIController = GetComponent<DeployUIController>();    
        turnBeginUIController = GetComponent<TurnBeginUIController>();
        chooseActionUIController = GetComponent<ChooseActionUIController>();
        skillSelectionUIController = GetComponent<SkillSelectionUIController>();
        arrowSelectionUIController = GetComponent<ArrowSelectionUIController>();
        skillTargetUIController = GetComponent<SkillTargetUIController>();
        stageClearUIController = GetComponent<StageClearUIController>();
    }

    /**********************************************************
    * ��ġ������ ���� ����
    ***********************************************************/
    public void CreateDeploySlot()
    {
        foreach(var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(deploySlot, deployWindow);

            var slot = ob.GetComponent<DeploySlot>();
            slot.image.sprite = unitSmallPool.images[kvp.Key];
            slot.className.text = kvp.Key;
            slot.level.text = kvp.Value.Level.ToString();
            slot.hp.text = kvp.Value.HP.ToString();

            deployUIController.unitButtons.Add(kvp.Key, ob.GetComponent<Button>());
        }
    }

    /**********************************************************
    * ���â ����
    ***********************************************************/
    public void CreateResultSlot()
    {
        int exp = BattleMapManager.instance.GetReward();
        
        foreach(var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(resultSlot, resultWindow);

            var slot = ob.GetComponent<ResultSlot>();
            slot.image.sprite = unitSmallPool.images[kvp.Key];
            slot.className.text = kvp.Key;
            slot.level.text = kvp.Value.Level.ToString();
            slot.exp.text = kvp.Value.reqEXP.ToString();
            // ���� exp �߰�
            // ������ ���� �߰�
        }



    }

    private IEnumerator UpExp(int exp)
    {


        yield return null;
    }
}
