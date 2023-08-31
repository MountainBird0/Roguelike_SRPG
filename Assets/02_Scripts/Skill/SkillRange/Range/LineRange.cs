using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRange : MonoBehaviour
{
    public List<TileLogic> GetTilesInRange(Board board, int range)
    {
        Vector3Int currentPos = Turn.selectedTile.pos;
        
        List<TileLogic> tileLogics = new();
        TileLogic tile;


        for (int i = 1; i <= range; i++)
        {
            currentPos += Turn.direction;
            tile = board.GetTile(currentPos);
            tileLogics.Add(tile);
        }

        return tileLogics;
    }
}
