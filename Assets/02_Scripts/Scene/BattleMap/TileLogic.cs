using UnityEngine;

public class TileLogic : MonoBehaviour
{
    public Vector3Int pos;
    public Vector3 worldPos;
    public GameObject content;

    #region pathfinding
    public TileLogic prev;
    public float distance;
    #endregion

    //public TileType tileType;

    public TileLogic() { }

    public TileLogic(Vector3Int cellPos, Vector3 worldPosition)
    {
        pos = cellPos;
        worldPos = worldPosition;
    }

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
