/**********************************************************
* 배틀 맵의 입력 관리
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleMapInput : MonoBehaviour
{
    // test
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tilemap hight;
    [SerializeField]
    private TileBase newTile;

    public GameObject player;

    private void OnEnable()
    {
        InputManager.instance.OnStartTouch += ScreenTouch;
    }

    private void OnDisable()
    {
        InputManager.instance.OnStartTouch -= ScreenTouch;
    }

    private void ScreenTouch(Vector2 screenPosition, float time)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.x += 0.5f;
        worldPosition.y += 0.5f;
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            Debug.Log($"{GetType()} - 터치한 타일: {tile.name}, {tile}");
            hight.SetTile(cellPosition, newTile);
            player.transform.position = cellPosition;
        }
    }
}
