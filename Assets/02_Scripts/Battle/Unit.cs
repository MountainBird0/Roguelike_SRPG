/**********************************************************
* 각 유닛의 정보 저장
***********************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitNum;

    public GameObject body;
    public Sprite image;
    public Image redBar;

    public PlayerType playerType;
    public AllianceColorType allianceType;
    public int faction;

    [HideInInspector]
    public bool isTurnEnd = false; // 안쓸듯 -> 다른방법이 있나

    public Vector3Int pos;

    public List<GameObject> skills;
    public List<int> coolTimeList;

    // 유닛 방향 좌 우
    public TileLogic tile; // 이거 그냥 vec3로 바꾸거나 할수도

    public StatData maxStats; // 레벨업 등 할 때 마다 maxStat 갱신해서 사용하기
    public StatData stats;

    public AnimationController animationController;

    private void Start()
    {
        animationController = GetComponent<AnimationController>();
    }


    public void SetStat()
    {
        
    }



    public void SetHealthBar()
    {
        float hpRatio = (float)stats.HP / stats.MaxHP;
        redBar.fillAmount = hpRatio;
    }

    public void Die()
    {

    }


}

// 배틀맵 매니저 AddUnit때 스킬도 넣을까 