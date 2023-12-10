/**********************************************************
* �� Ÿ���� ���� �� 
***********************************************************/
using UnityEngine;

public class TileLogic
{
    public Vector3Int pos;
   // public Vector3 worldPos; // worldPos ��������
    public GameObject content; // Ÿ������ ������Ʈ

    public TileLogic prev;
    public float distance;

    //public TileType tileType;

    public TileLogic() { }

    public TileLogic(Vector3Int cellPos)
    {
        pos = cellPos;
    }

    public Quaternion? GetDirection(TileLogic targetTile)
    {
        return GetDirection(targetTile.pos);
    }
    public Quaternion? GetDirection(Vector3Int targetTile)
    {
        if (this.pos.x > targetTile.x)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        else if (this.pos.x < targetTile.x)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        return null;
    }
}
