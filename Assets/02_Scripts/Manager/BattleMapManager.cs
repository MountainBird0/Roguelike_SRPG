/**********************************************************
* 배틀 맵 관리
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
    public Board board; // 생성된 맵에서 불러올 board

    [HideInInspector]
    public List<Unit> units; // 유닛 리스트

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
    * 맵로딩
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
    }

    /**********************************************************
    * 유닛 리스트에 플레이어 넣기
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        string unitName = unit.unitName;

        unit.tile = TL; // 이거 쓸까
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
        // 몬스터도 나중에 추가
    }

    /**********************************************************
    * 유닛 리스트에 몬스터 넣기
    ***********************************************************/
    public void AddMonster(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        string unitName = unit.unitName;

        unit.tile = TL; // 이거 쓸까
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
    * 유닛 리스트에 Player있는지 확인
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
