/**********************************************************
* �� ������ ���� ����
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
    public bool isTurnEnd = false; // �Ⱦ��� -> �ٸ������ �ֳ�

    public Vector3Int pos;

    public List<GameObject> skills;
    public List<int> coolTimeList;

    // ���� ���� �� ��
    public TileLogic tile; // �̰� �׳� vec3�� �ٲٰų� �Ҽ���

    public StatData maxStats; // ������ �� �� �� ���� maxStat �����ؼ� ����ϱ�
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

// ��Ʋ�� �Ŵ��� AddUnit�� ��ų�� ������ 