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
    private MapSelector selector;
    public Transform map;

    [Header("Machine")]
    public RangeSearchMachine rangeSearchMachine;
    public AIController aiController;

    [Header("SkillPool")]
    public SkillPool skillPool;

    [HideInInspector]
    public Board board; // ������ �ʿ��� �ҷ��� board

    private int unitNum;
    private Dictionary<int, Unit> allyUnits = new();
    private Dictionary<int, Unit> enemyUnits = new();

    public Dictionary<int, Unit> HumanUnits = new();
    public Dictionary<int, Unit> AIUnits = new();


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
        selector = GetComponent<MapSelector>();
        board = selector.SelectMap(map).GetComponent<Board>();
        rangeSearchMachine.SetBoard(board);
        aiController.SetMachineBoard(rangeSearchMachine, board);
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
            skillList = DataManager.instance.currentEquipSkills[name];
            allyUnits.Add(unit.unitNum, unit);
        }
        else if(DataManager.instance.defaultMonsterStats.ContainsKey(name))
        {
            unit.stats = DataManager.instance.defaultMonsterStats[name];
            skillList = DataManager.instance.defaultMonsterEquipSkills[name];
            enemyUnits.Add(unit.unitNum, unit);
        }

        for (int i = 0; i < skillList.Count; i++)
        {
            GameObject ob;
            if (skillList[i].Equals(-1))
            {
                ob = skillPool.skills.Last();
            }
            else
            {
                ob = Instantiate(skillPool.skills[skillList[i]]);
                ob.GetComponent<Skill>().data = DataManager.instance.defaultSkillStats[skillList[i]];
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
        foreach(var kvp in allyUnits)
        {
            kvp.Value.isTurnEnd = false;
        }
        foreach (var kvp in AIUnits)
        {
            kvp.Value.isTurnEnd = false;
        }

        HumanUnits = new(allyUnits);
        AIUnits = new(enemyUnits); 
    }

    /**********************************************************
    * �ൿ ���� �Ʊ����� ���� �Ʊ��� �� �������� Ȯ��
    ***********************************************************/
    public bool isHumanTurnFinish(int num)
    {
        if(HumanUnits.ContainsKey(num))
        {
            HumanUnits[num].isTurnEnd = true;
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
            AIUnits[num].isTurnEnd = true;
            AIUnits.Remove(num);
        }

        if(AIUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - ������� ��");
            ResetUnit();
            return true;
        }
        return false;
    }

    /**********************************************************
    * �������� ����
    ***********************************************************/
    public void DeleteUnit(int unitNum)
    {
        if (allyUnits.ContainsKey(unitNum))
        {
            board.mainTiles[allyUnits[unitNum].pos].content = null;
            allyUnits.Remove(unitNum);
        }
        else if (enemyUnits.ContainsKey(unitNum))
        {
            board.mainTiles[enemyUnits[unitNum].pos].content = null;
            enemyUnits.Remove(unitNum);
        }

        if(HumanUnits.ContainsKey(unitNum))
        {
            HumanUnits.Remove(unitNum);
        }
        else if(AIUnits.ContainsKey(unitNum))
        {
            AIUnits.Remove(unitNum);
        }

        

    }

    /**********************************************************
    * ���� Ŭ����� Ȯ��
    ***********************************************************/
    public bool? ClearCheck()
    {
        if (allyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - �й�");         
            return false;
        }
        else if (enemyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - �¸�");          
            return true;
        }
        return null;
    }

    /**********************************************************
    * ���� �ʿ� �ִ� ��� ������ �ҷ���
    ***********************************************************/
    public List<Unit> GetAllUnit()
    {
        List<Unit> units = new();

        foreach (var kvp in allyUnits)
        {
            units.Add(kvp.Value);
        }
        foreach (var kvp in enemyUnits)
        {
            units.Add(kvp.Value);
        }

        return units;
    }

    /**********************************************************
    * ���� ���
    ***********************************************************/
    public int GetReward()
    {
        int exp = 0;

        foreach(var kvp in enemyUnits)
        {
            exp += kvp.Value.stats.dropEXP;
        }

        return exp;
    }

}
