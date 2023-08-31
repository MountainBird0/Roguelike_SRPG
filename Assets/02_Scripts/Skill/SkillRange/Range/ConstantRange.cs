using UnityEngine;

public class ConstantRange : MonoBehaviour
{
    public bool SearchType(TileLogic from, TileLogic to, int range)
    {
        to.distance = from.distance + 1;

        return to.distance <= range;
    }
}
