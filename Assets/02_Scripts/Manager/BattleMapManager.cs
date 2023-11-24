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
    public AIController aiController;

    [Header("SkillPool")]
    public SkillPool skillPool;

    [HideInInspector]
    public Board board; // 생성된 맵에서 불러올 board

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

        Debug.Log($"{GetType()} - 실행");
        unitNum = 0;
    }

    /**********************************************************
    * 맵로딩
    ***********************************************************/
    public void MapLoad()
    {
        selector.SelectMap(map);

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();
        rangeSearchMachine.SetBoard(board);
        aiController.SetMachineBoard(rangeSearchMachine, board);
    }

    /**********************************************************
    * 생성된 유닛 스텟, 스킬 세팅하기
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
            Debug.Log($"{GetType()} - 들어간유닛수{allyUnits.Count}");
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
                // ob = Instantiate(skillPool.skills.Last());
                ob = skillPool.skills.Last();
            }
            else
            {
                // ob = Instantiate(skillPool.skills[skillList[i]]);
                ob = skillPool.skills[skillList[i]];
                ob.GetComponent<Skill>().data = DataManager.instance.defaultSkillStats[skillList[i]];
            }
            ob.GetComponent<Skill>().id = skillList[i];
            
            unit.skills.Add(ob);
        }       
    }


    /**********************************************************
    * 행동 가능한 유닛 초기화
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
    * 행동 끝난 아군유닛 빼고 아군턴 다 끝났는지 확인
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
    * 행동 끝난 적 유닛 빼고 턴 끝났는지 확인
    ***********************************************************/
    public bool isAITurnFinish(int num)
    {
        if(AIUnits.ContainsKey(num))
        {
            Debug.Log($"{GetType()} - 유닛지움");
            AIUnits[num].isTurnEnd = true;
            AIUnits.Remove(num);
        }

        if(AIUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - 모든유닛 끝");
            ResetUnit();
            return true;
        }
        return false;
    }

    /**********************************************************
    * 죽은유닛 빼기
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
    * 게임 클리어여부 확인
    ***********************************************************/
    public void ClearCheck()
    {
        if (allyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - 패배");
            StateMachineController.instance.ChangeTo<StageEndState>();
        }
        else if (enemyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - 승리");
            StateMachineController.instance.ChangeTo<StageEndState>();
        }
    }

    /**********************************************************
    * 현재 맵에 있는 모든 유닛을 불러옴
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

}
