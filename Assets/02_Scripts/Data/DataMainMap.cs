/******************************************************************************
* 메인 맵 데이터 관리
*******************************************************************************/
using System.Collections.Generic;

public class DataMainMap
{
    // 프로퍼티 쓰는 이유
    public int stageNum { get; set; }                            // 스테이지
    public Queue<int> iconNums { get; set; }                     // 라인마다 들어갈 아이콘의 수
    public List<(int icon, bool check)> iconStates { get; set; } // 아이콘 종류와 활성화 여부

    // public int icon { get; set; }
    // public byte check { get; set; }
}

//public class icon
//{
//    int 종류;
//    int 라인 몇라인인지
//    bool 활성화;
//}



