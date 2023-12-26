using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeRange : MonoBehaviour
{
    public List<TileLogic> GetTilesInRange(Board board, Vector3Int currentPos, int range)
    {
        Vector3Int next;

        List<TileLogic> tileResult = new();
        TileLogic tile;

        int lateral = 1;
        int min = 0;
        int max = 0;


        for (int i = 1; i <= range; i++)
        {
            min = - (lateral / 2);
            max = (lateral / 2);

            for(int j = min; j <= max; j++)
            {
                next = GetNext(currentPos, i, j);
                tile = board.GetTile(next);
                if(tile == null)
                {
                    continue;
                }
                else
                {
                   tileResult.Add(tile);
                }
            }
            lateral += 2;
        }

        return tileResult;
    }

    private Vector3Int GetNext(Vector3Int currentPos, int arg1, int arg2)
    {
        Vector3Int next = Vector3Int.zero;
        
        if(Turn.direction.y.Equals(1))
        {
            next = new Vector3Int(currentPos.x + arg2, currentPos.y + arg1, 0);
        }
        else if (Turn.direction.y.Equals(-1))
        {
            next = new Vector3Int(currentPos.x + arg2, currentPos.y - arg1, 0);
        }
        else if (Turn.direction.x.Equals(1))
        {
            next = new Vector3Int(currentPos.x + arg1, currentPos.y + arg2, 0);
        }
        else if(Turn.direction.x.Equals(-1))
        {
            next = new Vector3Int(currentPos.x - arg1, currentPos.y + arg2, 0);
        }

        return next;
    }

}
