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
        //worldPos = worldPosition;
    }
}
