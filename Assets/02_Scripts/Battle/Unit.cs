/**********************************************************
* 각 유닛의 정보 저장
***********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitNum;

    public GameObject body;
    public Sprite image;
    public Image redBar;

    public PlayerType playerType;
    public AllianceColorType allianceType;
    public int faction;

    [HideInInspector]
    public bool isTurnEnd = false; // 안쓸듯 -> 다른방법이 있나

    public TileLogic tile; // 이거 그냥 vec3로 바꾸거나 할수도
    public Vector3Int pos;

    public List<GameObject> skills;
    public List<int> coolTimeList;

    // 유닛 방향 좌 우

    public StatData maxStats; // 레벨업 등 할 때 마다 maxStat 갱신해서 사용하기
    public StatData stats;

    public AnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
    }

    public void SetPosition(Vector3Int pos, Board board)
    {
        var realPos = Turn.unit.pos;

        if (pos != realPos)
        {
            board.mainTiles[pos].content = board.mainTiles[realPos].content;
            board.mainTiles[realPos].content = null;
        }
        
        transform.position = pos;
        this.pos = pos;
    }

    public void SetStat()
    {
        
    }



    public void SetHealth(int value)
    {
        stats.HP += value;

        if(stats.HP > stats.MaxHP)
        {
            stats.HP = stats.MaxHP;
        }

        SetHealthBar();

        if(stats.HP <= 0)
        {
            stats.HP = 1;
            Die();
        }
    }

    public void SetHealthBar()
    {
        float hpRatio = (float)stats.HP / stats.MaxHP;
        redBar.fillAmount = hpRatio;
    }



    public void Die()
    {
        Debug.Log($"{GetType()} - 디짐");
        BattleMapManager.instance.DeleteUnit(unitNum);
        animationController.Death();
    }

    public void DespawnUnit()
    {
        ObjectPoolManager.instance.Despawn(this.gameObject);
    }

    public bool ISMovable(TileLogic from, TileLogic to)
    {
        to.distance = from.distance + 1;

        return ((to.content == null || to.content.GetComponent<Unit>() == this)
            && to.distance <= stats.MOV);
    }


}

// 배틀맵 매니저 AddUnit때 스킬도 넣을까 