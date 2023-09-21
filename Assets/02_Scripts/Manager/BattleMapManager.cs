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

    [HideInInspector]
    public List<Unit> units; // ���� ����Ʈ

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
    * �ʷε�
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
    }

    /**********************************************************
    * ���� ����Ʈ�� �÷��̾� �ֱ�
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        string unitName = unit.unitName;

        unit.tile = TL; // �̰� ����
        if(DataManager.instance.currentUnitStats.ContainsKey(unitName))
        {
            var skillList = DataManager.instance.currentEquipSkills[unitName].list;
            for(int i = 0; i < skillList.Count; i++)
            {
                GameObject ob;
                
                if(skillList[i].Equals(-1))
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
            unit.stats = DataManager.instance.currentUnitStats[unitName];
            unit.maxStats = unit.stats;
        }
        // ���͵� ���߿� �߰�
    }

    /**********************************************************
    * ���� ����Ʈ�� ���� �ֱ�
    ***********************************************************/
    public void AddMonster(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        string unitName = unit.unitName;

        unit.tile = TL; // �̰� ����
        if (DataManager.instance.defaultMonsterStats.ContainsKey(unitName))
        {
            var skillList = DataManager.instance.defaultMonsterEquipSkills[unitName].list;
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
                unit.skills.Add(ob);
            }
        }
    }

    /**********************************************************
    * ���� ����Ʈ�� Player�ִ��� Ȯ��
    ***********************************************************/
    public bool IsHuman()
    {
        for(int i = 0; i < units.Count; i++)
        {
            if(units[i].playerType == PlayerType.HUMAN)
            {
                return true;
            }
        }
        return false;
    }
}
