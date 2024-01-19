/**********************************************************
* BattleMap의 UI 관리, 생성 등
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public BattleMapUIController battleMapUIController;
    public DeployUIController deployUIController;
    public TurnBeginUIController turnBeginUIController;
    public ChooseActionUIController chooseActionUIController;
    public SkillSelectionUIController skillSelectionUIController;
    public ArrowSelectionUIController arrowSelectionUIController;
    public SkillTargetUIController skillTargetUIController;
    public TurnEndUIController turnEndUIController;
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

        battleMapUIController = GetComponent<BattleMapUIController>();
        deployUIController = GetComponent<DeployUIController>();    
        turnBeginUIController = GetComponent<TurnBeginUIController>();
        chooseActionUIController = GetComponent<ChooseActionUIController>();
        skillSelectionUIController = GetComponent<SkillSelectionUIController>();
        arrowSelectionUIController = GetComponent<ArrowSelectionUIController>();
        skillTargetUIController = GetComponent<SkillTargetUIController>();
        turnEndUIController = GetComponent<TurnEndUIController>();
        stageClearUIController = GetComponent<StageClearUIController>();
    }

    /**********************************************************
    * 배치가능한 유닛 생성
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
    * 결과창 생성
    ***********************************************************/
    public void CreateResultSlot()
    {
        int gainedExp = BattleMapManager.instance.GetReward();
        
        foreach(var unit in BattleMapManager.instance.units)
        {
            var ob = Instantiate(resultSlot, resultWindow);

            var slot = ob.GetComponent<ResultSlot>();
            slot.image.sprite = unitSmallPool.images[unit.unitName];
            slot.className.text = unit.unitName;
            slot.level.text = unit.stats.Level.ToString();
            slot.curExp.text = unit.stats.CurEXP.ToString();
            slot.maxExp.text = "/ " + unit.stats.MaxEXP.ToString();

            StartCoroutine(UpExp(unit, slot, gainedExp));
        }
    }

    private IEnumerator UpExp(Unit unit, ResultSlot slot, int gainedExp)
    {
        int currentEXP = unit.stats.CurEXP;
        int addExp = 0;
        gainedExp = 100; // 지울꺼////////////////////////////////////
        while (gainedExp != 0)
        {
            int requirExp = unit.stats.MaxEXP - currentEXP;

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

            for (int i = 0; i < addExp; i++)
            {
                yield return new WaitForSeconds(0.01f);
                currentEXP++;
                slot.curExp.text = currentEXP.ToString();
                slot.yellowBar.fillAmount = (float)currentEXP / unit.stats.MaxEXP;
            }
            yield return new WaitForSeconds(0.05f);

            if (currentEXP == unit.stats.MaxEXP)
            {
                unit.stats = unit.stats.IncreaseLevel(DataManager.instance.defaultUnitGrowStats[unit.unitName]);

                slot.level.text = unit.stats.Level.ToString();
                slot.curExp.text = "0";
                slot.maxExp.text = unit.stats.MaxEXP.ToString();

                currentEXP = 0;
                
                slot.yellowBar.fillAmount = (float)currentEXP / unit.stats.MaxEXP;
            }
        }

        unit.stats.CurEXP = int.Parse(slot.curExp.text, CultureInfo.InvariantCulture);

        DataManager.instance.currentUnitStats[unit.unitName] = unit.stats;

        yield return new WaitForSeconds(0.1f);
        // 이거 끝나야 나머지 활성화되도록

        stageClearUIController.EnableEndButton();
    }

    /**********************************************************
    * 턴 ui 세팅
    ***********************************************************/
    public void SetTurnCount()
    {
        battleMapUIController.SetTurnCount();
    }

}
