/**********************************************************
* ��Ʋ ���� �Է� ����
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BattleMapInput : MonoBehaviour
{
    Ray ray;
    RaycastHit2D hit;

    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Tilemap hight;
    [SerializeField]

    private TileBase newTile;

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
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        TileBase tile = tilemap.GetTile(cellPosition);

        if (tile != null)
        {
            Debug.Log($"{GetType()} - ��ġ�� Ÿ��: {tile.name}");
            hight.SetTile(cellPosition, newTile);
        }
    }
}
