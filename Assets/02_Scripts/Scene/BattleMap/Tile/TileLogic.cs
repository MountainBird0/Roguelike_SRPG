/**********************************************************
* 각 타일의 정보 등 
***********************************************************/
using UnityEngine;

public class TileLogic
{
    public Vector3Int pos;
   // public Vector3 worldPos; // worldPos 삭제가능
    public GameObject content; // 타일위의 오브젝트

    public TileLogic prev;
    public float distance;

    //public TileType tileType;

    public TileLogic() { }

    public TileLogic(Vector3Int cellPos)
    {
        pos = cellPos;
        //worldPos = worldPosition;
    }
}
