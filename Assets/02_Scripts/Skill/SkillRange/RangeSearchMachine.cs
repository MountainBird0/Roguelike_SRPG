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

    private Board board;

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public List<TileLogic> SearchRange(Vector3Int pos, SkillData data, bool isAOE)
    {
        int range;
        if (isAOE)
        {
            range = data.AOERange;
        }
        else
        {
            range = data.range;
        }
        RangeType rangeType = data.rangeType;

        switch (rangeType)
        {
            case RangeType.CONSTANT:
                //return board.Search(board.GetTile(pos), range, constantRange.SearchType);
                return constantRange.GetTilesInRange(board, pos, range);

            case RangeType.LINE:
                return lineRange.GetTilesInRange(board, pos, range);

            case RangeType.CONE:
                return coneRange.GetTilesInRange(board, pos, range);

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
