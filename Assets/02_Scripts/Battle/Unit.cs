/**********************************************************
* �� ������ ���� ����
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
    public bool isTurnEnd = false; // �Ⱦ��� -> �ٸ������ �ֳ�

    public TileLogic tile; // �̰� �׳� vec3�� �ٲٰų� �Ҽ���
    public Vector3Int pos;

    public List<GameObject> skills;
    public List<int> coolTimeList;

    // ���� ���� �� ��

    public StatData maxStats; // ������ �� �� �� ���� maxStat �����ؼ� ����ϱ�
    public StatData stats;

    public AnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
    }


    public void SetPosition(Vector3Int pos, Board board)
    {
        if (pos != Turn.unit.pos)
        {
            board.mainTiles[pos].content = board.mainTiles[Turn.unit.pos].content;
            board.mainTiles[Turn.unit.pos].content = null;
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
        if(stats.HP <= 0)
        {
            SetHealthBar();
            Die();
        }

        SetHealthBar();
    }

    private void SetHealthBar()
    {
        float hpRatio = (float)stats.HP / stats.MaxHP;
        redBar.fillAmount = hpRatio;
    }



    public void Die()
    {
        Debug.Log($"{GetType()} - ����");
        BattleMapManager.instance.DeleteUnit(unitNum);
        animationController.Death();
    }

    public void DespawnUnit()
    {
        ObjectPoolManager.instance.Despawn(this.gameObject);
    }


}

// ��Ʋ�� �Ŵ��� AddUnit�� ��ų�� ������ 