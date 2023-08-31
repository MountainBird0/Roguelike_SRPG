/**********************************************************
* 각 유닛의 정보 저장
***********************************************************/
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public Sprite smallIcon;
    public Sprite BigIcon;

    // 유닛 방향 좌 우

    public TileLogic tile; // 이거 그냥 vec3로 바꾸거나 할수도

    public StatData maxStats; // 레벨업 등 할 때 마다 maxStat 갱신해서 사용하기
    public StatData stats;

    public PlayerType playerType;
    public AllianceType allianceType;
    // 동맹 추가
    public int faction;
}

