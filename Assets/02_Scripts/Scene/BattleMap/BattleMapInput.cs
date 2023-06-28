/**********************************************************
* 배틀 맵의 입력 관리
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
        ray = Camera.main.ScreenPointToRay(screenPosition);
        hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {         
            Tilemap hitTilemap = hit.collider.GetComponent<Tilemap>();

            if (hitTilemap != null && hitTilemap == tilemap)
            {
                Vector3Int cellPosition = tilemap.WorldToCell(hit.point);
                TileBase tile = tilemap.GetTile(cellPosition);

                if (tile != null)
                {
                    Debug.Log($"{GetType()} - 터치한 타일: {tile.name}");
                }
            }
        }
    }
}
