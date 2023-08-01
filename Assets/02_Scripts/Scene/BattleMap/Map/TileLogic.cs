using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLogic
{
    public Vector3Int pos;
    public Vector3 worldPos;
    public GameObject content; // 타일위의 오브젝트

    public TileLogic prev;
    public float distance;

    //public TileType tileType;

    public TileLogic() { }

    public TileLogic(Vector3Int cellPos, Vector3 worldPosition)
    {
        pos = cellPos;
        worldPos = worldPosition;
    }

    // 이거 어따쓰는지
    public char GetDirection(TileLogic t2)
    {
        return GetDirection(t2.pos);
    }
    public char GetDirection(Vector3Int t2)
    {
        if (this.pos.y < t2.y)
            return 'N';
        if (this.pos.x < t2.x)
            return 'E';
        if (this.pos.y > t2.y)
            return 'S';
        return 'W';
    }
}
