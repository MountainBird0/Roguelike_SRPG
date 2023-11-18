/**********************************************************
* ��Ʋ �� ����
***********************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    public static BattleMapManager instance;

    [Header("Map")]
    public MapSelector selector;
    public Transform map;

    [Header("Machine")]
    public RangeSearchMachine rangeSearchMachine;

    [Header("SkillPool")]
    public SkillPool skillPool;

    [HideInInspector]
    public Board board; // ������ �ʿ��� �ҷ��� board

    private int unitNum;
    private Dictionary<int, Unit> allyUnits = new();
    private Dictionary<int, Unit> enemyUnits = new();

    private Dictionary<int, Unit> HumanUnits = new();
    private Dictionary<int, Unit> AIUnits = new();


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

        Debug.Log($"{GetType()} - ����");
        unitNum = 0;
    }

    /**********************************************************
    * �ʷε�
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
    }

    /**********************************************************
    * ������ ���� ����, ��ų �����ϱ�
    ***********************************************************/
    public void UnitSetting(Unit unit)
    {
        string name = unit.unitName;
        unit.unitNum = unitNum++;
        List<int> skillList = null;

        if (DataManager.instance.currentUnitStats.ContainsKey(name))
        {
            unit.stats = DataManager.instance.currentUnitStats[name];
            skillList = DataManager.instance.currentEquipSkills[name].list;
            allyUnits.Add(unit.unitNum, unit);
        }
        else if(DataManager.instance.defaultMonsterStats.ContainsKey(name))
        {
            unit.stats = DataManager.instance.defaultMonsterStats[name];
            skillList = DataManager.instance.defaultMonsterEquipSkills[name].list;
            enemyUnits.Add(unit.unitNum, unit);
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            GameObject ob;
            if (skillList[i].Equals(-1))
            {
                ob = Instantiate(skillPool.skills.Last());
            }
            else
            {
                ob = Instantiate(skillPool.skills[skillList[i]]);
            }
            ob.GetComponent<Skill>().id = skillList[i];
            unit.skills.Add(ob);
        }       
    }

    /**********************************************************
    * �ൿ ������ ���� �ʱ�ȭ
    ***********************************************************/
    public void ResetUnit()
    {
        HumanUnits = allyUnits;
        AIUnits = enemyUnits; 
    }



    /**********************************************************
    * �ൿ ���� �Ʊ����� ���� �Ʊ��� �� �������� Ȯ��
    ***********************************************************/
    public bool isHumanTurnFinish(int num)
    {
        if(HumanUnits.ContainsKey(num))
        {
            HumanUnits.Remove(num);
        }

        if(HumanUnits.Count.Equals(0))
        {
            return false;
        }
        return true;
    }
    /**********************************************************
    * �ൿ ���� �� ���� ���� �� �������� Ȯ��
    ***********************************************************/
    public bool isAITurnFinish(int num)
    {
        if(AIUnits.ContainsKey(num))
        {
            AIUnits.Remove(num);
        }

        if(AIUnits.Count.Equals(0))
        {
            ResetUnit();
            return true;
        }
        return false;
    }

    /**********************************************************
    * �������� ����
    ***********************************************************/
    public void DeleteUnit(Unit unit)
    {
        int num = unit.unitNum;

        if (allyUnits.ContainsKey(num))
        {
            allyUnits.Remove(num);
        }
        else if (enemyUnits.ContainsKey(num))
        {
            enemyUnits.Remove(num);
        }
    }

    /**********************************************************
    * ���� Ŭ����� Ȯ��
    ***********************************************************/
    public void GameCheck()
    {
        if (allyUnits.Count.Equals(0))
        {
            // �й�

        }
        else if (enemyUnits.Count.Equals(0))
        {
            // �¸�

        }
    }

}
