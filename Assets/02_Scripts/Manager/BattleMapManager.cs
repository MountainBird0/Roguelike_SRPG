/**********************************************************
* ��Ʋ �� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    public static BattleMapManager instance;

    [Header("Map")]
    public MapSelector selector;
    public Transform map;

    [Header("Machine")]
    public RangeSearchMachine rangeSearchMachine;


    [HideInInspector]
    public Board board; // ������ �ʿ��� �ҷ��� board

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
    * ���� ����Ʈ�� ���� �ֱ�
    ***********************************************************/
    public void AddUnit(Unit unit, TileLogic TL)
    {
        units.Add(unit);
        unit.tile = TL; // �̰� ����
        if(DataManager.instance.currentUnitStats.ContainsKey(unit.unitName))
        {
            unit.stats = DataManager.instance.currentUnitStats[unit.unitName];
        }
        // ���͵� ���߿� �߰�
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
