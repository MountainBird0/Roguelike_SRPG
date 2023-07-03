/**********************************************************
* 배틀 맵 관리
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapManager : MonoBehaviour
{
    private Board board;

    List<Vector3Int> tiles = new List<Vector3Int>();

    private void Awake()
    {
        GameObject Map = GameObject.FindGameObjectWithTag("Map");
        board = Map.GetComponent<Board>();

        // 타일맵 세팅
        tiles = board.LoadTiles();

        foreach (var t in tiles)
        {
            Debug.Log($"{GetType()} - 타일이름 위치 - {t}, {t.x}, {t.y}");
        }
    }



    private void Start()
    {
        
    }


}
