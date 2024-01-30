/**********************************************************
* 현재 생성되어있는 맵의 정보 저장
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MapInfo
{
    public List<(IconType, Vector2)> iconInfoList = new(); // 각 아이콘의 종류와 위치
    public List<IconState> iconStates = new();

    public List<(int, int)> nodeDatas = new();         // 이전노드 시작위치, 몇번 들어갈지
}

