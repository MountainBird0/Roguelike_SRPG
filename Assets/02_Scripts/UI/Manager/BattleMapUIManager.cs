/**********************************************************
* BattleMap�� UI ����, ���� ��
***********************************************************/
using System.Collections;
using System.Collections.Generic;
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
        int gainedExp = BattleMapManager.instance.GetReward();
        
        foreach(var kvp in DataManager.instance.currentUnitStats)
        {
            var ob = Instantiate(resultSlot, resultWindow);

            var slot = ob.GetComponent<ResultSlot>();
            slot.image.sprite = unitSmallPool.images[kvp.Key];
            slot.className.text = kvp.Key;
            slot.level.text = kvp.Value.Level.ToString();
            slot.curExp.text = kvp.Value.CurEXP.ToString();
            slot.maxExp.text = "/ " + kvp.Value.MaxEXP.ToString();

            StartCoroutine(UpExp(kvp, slot, gainedExp));
        }
    }

    private IEnumerator UpExp(KeyValuePair<string, StatData> kvp, ResultSlot slot, int gainedExp)
    {
        Debug.Log($"{GetType()} - ���� : {kvp.Key}");
        Debug.Log($"{GetType()} - ��������ġ : {gainedExp}");
        int currentEXP = kvp.Value.CurEXP;
        int addExp = 0;

        while(gainedExp != 0)
        {
            Debug.Log($"{GetType()} - ���� �� ����ġ : {gainedExp}");
            int requirExp = kvp.Value.MaxEXP - currentEXP;
            Debug.Log($"{GetType()} - �������� �ʿ��� ����ġ : {requirExp}");

            if(gainedExp >= requirExp)
            {
                gainedExp -= requirExp;
                addExp = requirExp;
            }
            else
            {
                addExp = gainedExp;
                gainedExp = 0;
            }

            Debug.Log($"{GetType()} - ��� �� ���� gain : {gainedExp}");
            Debug.Log($"{GetType()} - ��� �� ���� add : {addExp}");

            for (int i = 0; i < addExp; i++)
            {
                yield return new WaitForSeconds(0.01f);
                currentEXP++;
                slot.curExp.text = currentEXP.ToString();
                slot.yellowBar.fillAmount = (float)currentEXP / kvp.Value.MaxEXP;
            }
            yield return new WaitForSeconds(0.05f);
            Debug.Log($"{GetType()} - currentEXP : {currentEXP}");
            Debug.Log($"{GetType()} - maxexp : {kvp.Value.MaxEXP}");

            if (currentEXP == kvp.Value.MaxEXP)
            {
                Debug.Log($"{GetType()} - ������ ��");
                Debug.Log($"{GetType()} - ����������? {kvp.Value.Level}");

                kvp = new KeyValuePair<string, StatData>(kvp.Key, kvp.Value.IncreaseLevel(DataManager.instance.defaultUnitGrowStats[kvp.Key]));

                Debug.Log($"{GetType()} - ������? {kvp.Value.Level}");
                slot.level.text = kvp.Value.Level.ToString();
                slot.curExp.text = "0";
                slot.maxExp.text = "/ " + kvp.Value.MaxEXP.ToString();

                currentEXP = 0;

                DataManager.instance.currentUnitStats[kvp.Key] = kvp.Value;
            }

        }

        yield return new WaitForSeconds(0.1f);
        // �̰� ������ ������ Ȱ��ȭ�ǵ���
    }
}
