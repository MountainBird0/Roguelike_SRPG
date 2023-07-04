/**********************************************************
* ��Ʋ �� ����
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    private Board board;
    public MapSelector selector;

    List<Vector3Int> tiles = new List<Vector3Int>();

    private void Awake()
    {
        // ������ �����ϱ� selecter
        selector.SelectMap();

        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();


    }



    private void Start()
    {
        


        // Ÿ�ϸ� ����
        tiles = board.LoadTiles();

        foreach (var t in tiles)
        {
            Debug.Log($"{GetType()} - Ÿ���̸� ��ġ - {t}, {t.x}, {t.y}");
        }
    }


}
