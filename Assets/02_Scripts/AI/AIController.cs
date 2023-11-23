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
    * �� �� ����
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
    * ��ų ����
    ***********************************************************/
    private bool ChooseSkill()
    {
        int highestScore = 0;
        Debug.Log($"{GetType()} - �������� : {Turn.unit.name}");
        var skills = SortSkills(Turn.unit.skills);

        if (skills.Count.Equals(0)) return false; // �̵��Ϸ����� bool �� ����

        for (int i = 0; i < skills.Count; i++) // ��ų ��ȸ
        {
            Debug.Log($"{GetType()} - ��ų�̸� : {skills[i].data.name}");

            var targetUnits = SearchUnit(skills[i].data.affectType);
            if (targetUnits.Count.Equals(0)) continue; // Ÿ�� ������ �ϳ��� ������ ���� ��ų�� �̵�

            for(int j = 0; j < targetUnits.Count; j++) // Ÿ�� ���� ��ȸ
            {
                Debug.Log($"{GetType()} - Ÿ�������̸� : {targetUnits[j].name}");
                var reachableTiles = SearchTileInRange(targetUnits[j].currentPos, skills[i].data);
                if (reachableTiles.Count.Equals(0)) continue; // ������ ��ų ������ �ϳ��� ������ ���� �������� �̵�
                Debug.Log($"{GetType()} - ������ ��ų ���� : {reachableTiles.Count}");
                for (int k = 0; k < reachableTiles.Count; k++)
                {
                    int score = 0;

                    if(skills[i].data.isDirectional)
                    {
                        Turn.direction = SearchDir(targetUnits[j].currentPos, reachableTiles[k], skills[i].data);                     
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
                        SetAIPlan(skills[i], reachableTiles[k].pos, targetUnits[j].currentPos);
                    }
                }
            }

            if (!highestScore.Equals(0)) return true;
        }

        return false;
    }

    /**********************************************************
    * ��Ÿ���� 0�� ��ų�� �켱������ �°� ����
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
    * ��ų������ �´� Ÿ�����ֵ��� ��ǥ�� ã�� 
    ***********************************************************/
    private List<Unit> SearchUnit(AffectType affectType)
    {
        List<Unit> validTargets = new();

        var units = BattleMapManager.instance.GetAllUnit();
        int originFaction = Turn.unit.GetComponent<Unit>().faction;
        Vector3Int originPos = Turn.unit.GetComponent<Unit>().currentPos;

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
            validTargets = validTargets.OrderBy(unit => Vector3Int.Distance(unit.currentPos, originPos)).ToList();
        }

        return validTargets;
    }

    /**********************************************************
    * Ÿ�� ������ �������� �ϴ� ��ų ���� ������
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
        List<TileLogic> moveableTiles = board.Search(board.GetTile(Turn.unit.currentPos), Turn.unit.stats.MOV, board.ISMovable);

        return tiles.Intersect(moveableTiles).ToList();      
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

        return targetUnits.Count(unit => tiles.Any(tile => tile.pos == unit.currentPos));
    }

    /**********************************************************
    * AIplan�� ���� �÷����� ����
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
        Debug.Log($"{GetType()} - 1 : {aiPlan.skill}");
        Debug.Log($"{GetType()} - 2 : {aiPlan.movePos}");
        Debug.Log($"{GetType()} - 3 : {aiPlan.targetPos}");
    }

    /**********************************************************
    * ���� ���� ����� Ÿ�Ϸ� �̵�
    ***********************************************************/
    private void MoveToEnemy()
    {
        Debug.Log($"{GetType()} - ���� �����Ÿ�Ϸ� �̵�");
    }
}
