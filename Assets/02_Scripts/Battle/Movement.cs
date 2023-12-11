using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Movement : MonoBehaviour
{
    const float MoveSpeed = 0.2f;

    TileLogic currentTile;

    public IEnumerator Move(List<TileLogic> path)
    {
        currentTile = Turn.unit.tile;

        for(int i = 0; i < path.Count; i++)
        {
            TileLogic next = path[i];
            
            if(currentTile.GetDirection(next).HasValue)
            {
                Turn.unit.body.transform.rotation = currentTile.GetDirection(next).Value;
            }

            Turn.unit.animationController.StartMove();
            yield return StartCoroutine(Walk(next));
        }

        yield return new WaitForSeconds(0.1f);
        Turn.unit.animationController.StopMove();
    }

    private IEnumerator Walk(TileLogic next)
    {
        currentTile = next;
        Tweener tweener = Turn.unit.transform.DOMove(currentTile.pos, MoveSpeed);
        yield return tweener.WaitForCompletion();
    }
    
}
