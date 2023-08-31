using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSearchMachine : MonoBehaviour
{
    [Header("RangeType")]
    public ConstantRange constantRange;
    public LineRange lineRange;
    public ConeRange coneRange;

    public List<TileLogic> SearchRange(Board board, Vector3Int pos, int range)
    {
        RangeType rangeType = (RangeType)Enum.Parse(typeof(RangeType), Turn.currentSkill.rangeType, true);

        switch (rangeType)
        {
            case RangeType.CONSTANT:
                return board.Search(board.GetTile(pos), range, constantRange.SearchType);

            case RangeType.LINE:
                return lineRange.GetTilesInRange(board, range);

            case RangeType.CONE:
                return coneRange.GetTilesInRange(board, range);

            case RangeType.INFINITE:
                break;


            case RangeType.SELF:
                break;
        }

        return null;
    }

    /**********************************************************
    * 방향선택 화살표 보이게 하기
    ***********************************************************/
    public void ShowArrow()
    {

    }



}
