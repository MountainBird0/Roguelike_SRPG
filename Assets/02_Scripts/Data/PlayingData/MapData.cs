using System.Collections.Generic;
using UnityEngine;

/**********************************************************
* 현재 생성되어있는 맵의 정보
***********************************************************/
public class MapData
{
    public int lineCount;    // 라인 수
    public List<int> iconCounts = new();  // 한 라인 당 아이콘 수

    public List<(IconType, Vector2)> iconInfo = new(); // 각 아이콘의 종류와 위치
    public List<IconState> iconStates = new();

    public List<(int, int)> nodeDatas = new();           // 이전노드 시작위치, 몇번 들어갈지
}

