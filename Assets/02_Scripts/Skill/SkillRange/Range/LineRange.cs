using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRange : MonoBehaviour
{
    public List<TileLogic> GetTilesInRange(Board board, Vector3Int currentPos, int range)
    {       
        List<TileLogic> tileResult = new();
        TileLogic tile;


        for (int i = 1; i <= range; i++)
        {
            currentPos += Turn.direction;
            tile = board.GetTile(currentPos);
            if (tile == null)
            {
                continue;
            }
            else
            {
                tileResult.Add(tile);
            }
        }

        return tileResult;
    }
}
