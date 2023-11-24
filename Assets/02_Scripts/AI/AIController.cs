using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public AIPlan aiPlan;

    private RangeSearchMachine searchMachine;
    private Board board;

    private Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    public void SetMachineBoard(RangeSearchMachine searchMachine, Board board)
    {
        this.searchMachine = searchMachine;
        this.board = board;
    }

    /**********************************************************
    * 할 것 고르기
    ***********************************************************/
    public void Evaluate()
    {
        aiPlan = null;

        if(!ChooseSkill())
        {
            MoveToEnemy();
        }
    }

    /**********************************************************
    * 스킬 고르기
    ***********************************************************/
    private bool ChooseSkill()
    {
        int highestScore = 0;
        Debug.Log($"{GetType()} - 현재 유닛 : {Turn.unit.name}");
        var skills = SortSkills(Turn.unit.skills);

        if (skills.Count.Equals(0)) return false; // 이동하러가기 bool 값 쓸듯

        for (int i = 0; i < skills.Count; i++) // 스킬 순회
        {
            Debug.Log($"{GetType()} - 순회할 스킬 이름 : {skills[i].name}");
            var targetUnits = SearchUnit(skills[i].data.affectType);
            if (targetUnits.Count.Equals(0)) continue; // 타겟 유닛이 하나도 없으면 다음 스킬로 이동

            for(int j = 0; j < targetUnits.Count; j++) // 타겟 유닛 순회
            {
                Debug.Log($"{GetType()} - 타겟 유닛 이름 : {targetUnits[j].name}");
                var reachableTiles = SearchTileInRange(targetUnits[j].pos, skills[i].data);
                if (reachableTiles.Count.Equals(0)) continue; // 가능한 스킬 범위가 하나도 없으면 다음 유닛으로 이동

                for (int k = 0; k < reachableTiles.Count; k++)
                {
                    int score = 0;

                    if(skills[i].data.isDirectional)
                    {
                        Turn.direction = SearchDir(targetUnits[j].pos, reachableTiles[k], skills[i].data);                     
                    }
                    else
                    {
                        Turn.direction = Vector3Int.zero;
                    }

                    if(skills[i].data.isAOE)
                    {
                        score = SearchAOETarget(targetUnits, reachableTiles[k], skills[i].data);
                    }
                    else
                    {
                        score = 1;
                    }
                    if (highestScore < score)
                    {
                        highestScore = score;
                        SetAIPlan(skills[i], reachableTiles[k].pos, targetUnits[j].pos);
                    }
                }
            }

            if (!highestScore.Equals(0)) return true;
        }

        return false;
    }

    /**********************************************************
    * 쿨타임이 0인 스킬을 우선순위에 맞게 정렬
    ***********************************************************/
    private List<Skill> SortSkills(List<GameObject> skills)
    {
        List<Skill> sortedSkills = skills
            .Select(ob => ob.GetComponent<Skill>())
            .Where(skill => skill.coolTime == 0)
            .OrderBy(skill => skill.data.affectType)
            .ToList();

        return sortedSkills;
    }

    /**********************************************************
    * 스킬종류에 맞는 타겟유닛들의 좌표를 찾음 
    ***********************************************************/
    private List<Unit> SearchUnit(AffectType affectType)
    {
        List<Unit> validTargets = new();

        var units = BattleMapManager.instance.GetAllUnit();
        int originFaction = Turn.unit.GetComponent<Unit>().faction;
        Vector3Int originPos = Turn.unit.GetComponent<Unit>().pos;

        for (int i = 0; i < units.Count; i++)
        {
            switch (affectType)
            {
                case AffectType.HEAL:
                    if(originFaction.Equals(units[i].faction))
                    {
                        if(!units[i].stats.HP.Equals(units[i].stats.MaxHP))
                        {
                            validTargets.Add(units[i]);
                        }
                    }
                    break;

                case AffectType.ATTACK:
                    if (!originFaction.Equals(units[i].faction))
                    {
                        validTargets.Add(units[i]);
                    }
                    break;

                case AffectType.BUFF:
                    if (originFaction.Equals(units[i].faction))
                    {
                        validTargets.Add(units[i]);
                    }
                    break;
            }
        }

        if (affectType == AffectType.HEAL)
        {
            validTargets = validTargets
              .OrderBy(unit => unit.stats.HP / unit.stats.MaxHP)
              .ToList();
        }
        else
        {
            validTargets = validTargets.OrderBy(unit => Vector3Int.Distance(unit.pos, originPos)).ToList();
        }

        return validTargets;
    }

    /**********************************************************
    * 타겟 유닛을 기준으로 하는 스킬 범위 저장함
    ***********************************************************/
    private List<TileLogic> SearchTileInRange(Vector3Int targetPos, SkillData data)
    {
        List<TileLogic> tiles = new();

        if (data.isDirectional)
        {
            for(int i = 0; i < dirs.Count(); i++)
            {
                Turn.direction = dirs[i];
                tiles.AddRange(searchMachine.SearchRange(targetPos, data, false));
            }
        }
        else
        {
            tiles = searchMachine.SearchRange(targetPos, data, false);
        }
        List<TileLogic> moveableTiles = board.Search(board.GetTile(Turn.unit.pos), Turn.unit.stats.MOV, board.ISMovable);

        return tiles.Intersect(moveableTiles).ToList();      
    }

    /**********************************************************
    * 방향 정하기
    ***********************************************************/
    private Vector3Int SearchDir(Vector3Int targetPos, TileLogic markTile, SkillData data)
    {
        Vector3Int dir = Vector3Int.zero;

        for (int l = 0; l < dirs.Count(); l++)
        {
            var checktiles = searchMachine.SearchRange(markTile.pos, data, false);

            TileLogic matchingTile = checktiles.FirstOrDefault(tile => tile.pos == targetPos);

            if (matchingTile != null)
            {
                dir = dirs[l];
                break;
            }
        }

        return dir;
    }

    /**********************************************************
    * 광역스킬 범위에 들어가는 타겟 찾기
    ***********************************************************/
    private int SearchAOETarget(List<Unit> targetUnits, TileLogic targetTile, SkillData data)
    {
        var tiles = searchMachine.SearchRange(targetTile.pos, data, true);

        return targetUnits.Count(unit => tiles.Any(tile => tile.pos == unit.pos));
    }

    /**********************************************************
    * AIplan을 현재 플랜으로 세팅
    ***********************************************************/
    private void SetAIPlan(Skill skill, Vector3Int movePos, Vector3Int targetPos)
    {
        if(aiPlan == null)
        {
            aiPlan = new();
        }

        aiPlan.skill = skill;
        aiPlan.movePos = movePos;
        aiPlan.targetPos = targetPos;
        Debug.Log($"{GetType()} - 정한 스킬 : {aiPlan.skill}");
        Debug.Log($"{GetType()} - 움직일 곳 : {aiPlan.movePos}");
        Debug.Log($"{GetType()} - 타겟 위치 : {aiPlan.targetPos}");
    }

    /**********************************************************
    * 적과 가장 가까운 타일로 이동
    ***********************************************************/
    private void MoveToEnemy()
    {
        Debug.Log($"{GetType()} - 가장 가까운타일로 이동");
    }
}
