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
    private MapSelector selector;
    public Transform map;

    [Header("Machine")]
    public RangeSearchMachine rangeSearchMachine;
    public AIController aiController;

    [Header("SkillPool")]
    public SkillPool skillPool;

    [Header("Camera")]
    public CameraController cameraController;

    [HideInInspector]
    public Board board; // 생성된 맵에서 불러올 board

    [HideInInspector]
    public bool isBtnAuto = false;


    private int unitNum;
    private Dictionary<int, Unit> allyUnits = new();
    private Dictionary<int, Unit> enemyUnits = new();

    public Dictionary<int, Unit> humanUnits = new();
    public Dictionary<int, Unit> AIUnits = new();

    public List<Unit> units = new(); // 처음 배치한 플레이어 유닛

    private int rewardExp = 0;

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
        selector = GetComponent<MapSelector>();
        board = selector.SelectMap(map).GetComponent<Board>();
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
            skillList = DataManager.instance.currentEquipSkills[name];
            allyUnits.Add(unit.unitNum, unit);
            units.Add(unit);
        }
        else if(DataManager.instance.defaultMonsterStats.ContainsKey(name))
        {
            unit.stats = DataManager.instance.defaultMonsterStats[name];
            skillList = DataManager.instance.defaultMonsterEquipSkills[name];
            enemyUnits.Add(unit.unitNum, unit);
        }

        unit.tile = board.GetTile(unit.pos);

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

        unit.SetHealthBar();
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

        humanUnits = new(allyUnits);
        AIUnits = new(enemyUnits); 
    }

    /**********************************************************
    * 행동 끝난 사람유닛 빼고 사람턴 다 끝났는지 확인
    ***********************************************************/
    public bool isHumanTurnFinish(int num)
    {
        if(humanUnits.ContainsKey(num))
        {
            humanUnits[num].isTurnEnd = true;
            humanUnits.Remove(num);
        }

        if(humanUnits.Count.Equals(0))
        {
            return false;
        }
        return true;
    }
    /**********************************************************
    * 행동 끝난 AI 유닛 빼고 턴 끝났는지 확인
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
            Debug.Log($"{GetType()} - 모든유닛 끝");
            ResetUnit();
            return true;
        }
        return false;
    }

    /**********************************************************
    * 적 턴 시작인지 확인
    ***********************************************************/
    public bool IsEnemyTurnStart()
    {
        if(enemyUnits.Count == AIUnits.Count)
        {
            return true;
        }

        return false;
    }

    /**********************************************************
    * 죽은유닛 빼기
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
            rewardExp += enemyUnits[unitNum].stats.dropEXP;

            board.mainTiles[enemyUnits[unitNum].pos].content = null;
            enemyUnits.Remove(unitNum);
        }

        if(humanUnits.ContainsKey(unitNum))
        {
            humanUnits.Remove(unitNum);
        }
        else if(AIUnits.ContainsKey(unitNum))
        {
            AIUnits.Remove(unitNum);
        }
    }

    /**********************************************************
    * 게임 클리어여부 확인
    ***********************************************************/
    public bool? ClearCheck()
    {
        if (allyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - 패배");         
            return false;
        }
        else if (enemyUnits.Count.Equals(0))
        {
            Debug.Log($"{GetType()} - 승리");          
            return true;
        }
        return null;
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


    /**********************************************************
    * 수동/자동 변경
    ***********************************************************/
    public void ChangeToAuto()
    {
        var sumDic = humanUnits.Concat(AIUnits);
        AIUnits = sumDic.ToDictionary(x => x.Key, x => x.Value);

        humanUnits.Clear();
    }
    public void ChangeToManual()
    {
        Dictionary<int,Unit> temp = new(AIUnits);

        foreach(var kvp in temp)
        {
            if(kvp.Value.faction == 0)
            {
                humanUnits.Add(kvp.Key, kvp.Value);
                AIUnits.Remove(kvp.Key);
            }
        }

        if(humanUnits.Count != 0)
        {
            Turn.isHumanTurn = true;
        }
    }




    /**********************************************************
    * 보상 계산
    ***********************************************************/
    public int GetReward()
    {
        return rewardExp;
    }

}
