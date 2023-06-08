/**********************************************************
* 플레이 중에 생기는 데이터들
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

/**********************************************************
* 게임의 기본 정보 - 시드, 현재 스테이지
***********************************************************/
public class GameInfo
{
    public int seed;
    public int currentStage;
}

/**********************************************************
* 현재 생성되어있는 맵의 정보
***********************************************************/
public class MapData
{
    public int lineCount;    // 라인 수
    public List<int> iconCounts = new List<int>();  // 한 라인 당 아이콘 수

    public List<(Icon, Vector2)> iconState = new List<(Icon, Vector2)>(); // 각 아이콘의 종류와 위치
    public List<(int, int)> nodeDatas = new List<(int, int)>();           // 이전노드 시작위치, 몇번 들어갈지
}

