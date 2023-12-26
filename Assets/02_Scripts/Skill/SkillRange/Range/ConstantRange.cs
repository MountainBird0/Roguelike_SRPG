using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRange : MonoBehaviour
{
    private Vector3Int[] dirs = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.down,
        Vector3Int.left,
        Vector3Int.right
    };

    public List<TileLogic> GetTilesInRange(Board board, Vector3Int currentPos, int range)
    {
        List<TileLogic> tileResult = new();
        var startTile = board.GetTile(currentPos);
        tileResult.Add(startTile);

        Dictionary<Vector3Int, int> posDic = new();
        posDic.Add(currentPos, 0);

        Queue<Vector3Int> checkNow = new();
        Queue<Vector3Int> checkNext = new();
        Vector3Int now = Vector3Int.zero;
        Vector3Int next = Vector3Int.zero;

        checkNow.Enqueue(currentPos);

        while (checkNow.Count > 0)
        {
            now = checkNow.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                next = now + dirs[i];

                if(posDic[now] + 1 >= range)
                {
                    continue;
                }

                if(!posDic.ContainsKey(next))
                {
                    posDic.Add(next, posDic[now] + 1);
                    checkNext.Enqueue(next);
                }

                TileLogic nextTile = board.GetTile(next);
                if (nextTile != null)
                {
                    tileResult.Add(nextTile);
                }
            }

            if(checkNow.Count == 0)
            {
                SwapReference(ref checkNow, ref checkNext);
            }
        }
        
        return tileResult;
    }

    private void SwapReference(ref Queue<Vector3Int> now, ref Queue<Vector3Int> next)
    {
        Queue<Vector3Int> temp = now;
        now = next;
        next = temp;
    }
}
