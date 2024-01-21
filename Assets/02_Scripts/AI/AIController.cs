using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public AIPlan currentPlan;

    private RangeSearchMachine searchMachine;
    private Board board;

    private Vector3Int nearestUnitPos;

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

    public AIPlan Evaluate()
    {
        currentPlan = null;

        return PlanChoice();
    }


    /**********************************************************
    * �� �� ����
    ***********************************************************/
    private AIPlan PlanChoice()
    {
        currentPlan = null;

        int highestScore = 0;
        //Debug.Log($"{GetType()} - ���� ���� : {Turn.unit.name}");
        var skills = SortSkills(Turn.unit.skills);

        if (skills.Count == 0) return null; // �̵��Ϸ����� bool �� ����

        for (int i = 0; i < skills.Count; i++) // ��ų ��ȸ
        {
            //Debug.Log($"{GetType()} - ��ȸ�� ��ų �̸� : {skills[i].name}");

            var targetUnits = SearchUnit(skills[i].data.affectType);
            if (targetUnits.Count == 0) continue; // Ÿ�� ������ �ϳ��� ������ ���� ��ų�� �̵�

            for(int j = 0; j < targetUnits.Count; j++) // Ÿ�� ���� ��ȸ
            {
                //Debug.Log($"{GetType()} - Ÿ�� ���� �̸� : {targetUnits[j].name}");
                var reachableTiles = SearchTileInRange(targetUnits[j].pos, skills[i].data);
                if (reachableTiles.Count.Equals(0)) continue; // ������ ��ų ������ �ϳ��� ������ ���� �������� �̵�

                for (int k = 0; k < reachableTiles.Count; k++)
                {
                    int score = CheckScore(skills[i], targetUnits[j], reachableTiles[k]);

                    if (highestScore < score)
                    {
                        highestScore = score;
                        SetAIPlan(skills[i], reachableTiles[k].pos, targetUnits[j].pos);
                    }
                }
            }

            if (highestScore != 0) return currentPlan;
        }

        return MoveToEnemy();
    }

    /**********************************************************
    * ���� ���
    ***********************************************************/
    private int CheckScore(Skill skill, Unit targetUnit, TileLogic reachableTile)
    {
        int score = 0;

        Turn.direction = skill.data.isDirectional ? SearchDir(targetUnit.pos, reachableTile, skill.data) : Vector3Int.zero;
        score = skill.data.isAOE ? SearchAOETarget(SearchUnit(skill.data.affectType), reachableTile, skill.data) : 1;

        return score;
    }

    /**********************************************************
    * ��Ÿ���� 0�� ��ų�� �켱������ �°� ����
    ***********************************************************/
    private List<Skill> SortSkills(List<GameObject> skills)
    {
        List<Skill> sortedSkills = skills
            .Select(ob => ob.GetComponent<Skill>())
            .Where(skill => skill.data.currentCoolTime == 0)
            .OrderByDescending(skill => skill.data.coolTime)
            .ThenBy(skill => skill.data.affectType)
            .ToList();

        return sortedSkills;
    }

    /**********************************************************
    * ��ų������ �´� Ÿ�����ֵ��� ��ǥ�� ã�� 
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

        if (affectType == AffectType.HEAL) // ü�� ���� �� ����
        {
            validTargets = validTargets
              .OrderBy(unit => unit.stats.HP / unit.stats.MaxHP)
              .ToList();
        }
        else // ����� �� ����
        {
            validTargets = validTargets.OrderBy(unit => Vector3Int.Distance(unit.pos, originPos)).ToList();
            nearestUnitPos = validTargets[0].pos;
        }

        return validTargets;
    }

    /**********************************************************
    * Ÿ�� ������ �������� �ϴ� ��ų ���� ������ // �̹� ������ �ִ� Ÿ���� ����
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
        List<TileLogic> moveableTiles = board.Search(board.GetTile(Turn.unit.pos), Turn.unit.ISMovable);

        tiles = tiles.Intersect(moveableTiles).ToList();
        tiles.Reverse();

        return tiles;
    }

    /**********************************************************
    * ���� ���ϱ�
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
    * ������ų ������ ���� Ÿ�� ã��
    ***********************************************************/
    private int SearchAOETarget(List<Unit> targetUnits, TileLogic targetTile, SkillData data)
    {
        var tiles = searchMachine.SearchRange(targetTile.pos, data, true);

        return targetUnits.Count(unit => tiles.Any(tile => tile.pos == unit.pos));
    }

    /**********************************************************
    * AIplan�� ���� �÷����� ����
    ***********************************************************/
    private void SetAIPlan(Skill skill, Vector3Int movePos, Vector3Int targetPos)
    {
        if(currentPlan == null)
        {
            currentPlan = new();
        }

        currentPlan.skill = skill;
        currentPlan.movePos = movePos;
        currentPlan.targetPos = targetPos;
        Debug.Log($"{GetType()} - ���� ��ų : {currentPlan.skill} / ������ �� : {currentPlan.movePos} / Ÿ�� ��ġ : {currentPlan.targetPos}");
    }

    /**********************************************************
    * ���� ���� ����� Ÿ�Ϸ� �̵�
    ***********************************************************/
    private AIPlan MoveToEnemy()
    {
        Debug.Log($"{GetType()} - ��ó�� �� ���� ���� �����Ÿ�Ϸ� �̵�");
        
        TileLogic targetTile = null;
        int currentFaction = Turn.unit.faction;

        board.Search(Turn.unit.tile, delegate(TileLogic arg1, TileLogic arg2) // ����� �� ã��
        {
            if (targetTile == null && arg2.content != null)
            {
                Unit unit = arg2.content.GetComponent<Unit>();
                if(unit != null && currentFaction != unit.faction)
                {
                    targetTile = arg2;
                    return true;
                }
            }
            arg2.distance = arg1.distance + 1;
            return targetTile == null;
        });


        if (currentPlan == null)
        {
            currentPlan = new();
        }

        currentPlan.skill = null;


        while (targetTile != Turn.unit.tile)
        {
            //if(targetTile.distance <= Turn.unit.stats.MOV &&
            //    targetTile.content == null)
            //{
            //    Debug.Log($"{GetType()} - ������ ��ġ ����{targetTile.pos}");
            //    currentPlan.movePos = targetTile.pos;
            //    break;
            //}
            if (board.highlightTiles.ContainsKey(targetTile.pos))
            {
                Debug.Log($"{GetType()} - ������ ��ġ ����{targetTile.pos}");
                currentPlan.movePos = targetTile.pos;
                break;
            }
            targetTile = targetTile.prev;
        }

        // ���� ����� ��η� �̵����� ����


        //// �̰� �ٸ������� �޾ƿ��� 
        //List<TileLogic> moveableTiles = board.Search(board.GetTile(Turn.unit.pos), Turn.unit.stats.MOV, board.ISMovable);
        //TileLogic closestTile = moveableTiles.OrderBy(tile => Vector3Int.Distance(tile.pos, nearestUnitPos)).FirstOrDefault();

        //if(closestTile != null)
        //{
        //    currentPlan.movePos = closestTile.pos;
        //}
        //else
        //{
        //    currentPlan.movePos = Turn.unit.pos;
        //}

        return currentPlan;
    }
}
