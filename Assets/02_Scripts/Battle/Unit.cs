/**********************************************************
* �� ������ ���� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public PlayerType playerType;
    public AllianceColorType allianceType;
    public int faction;

    public List<GameObject> skills;

    // ���� ���� �� ��
    public TileLogic tile; // �̰� �׳� vec3�� �ٲٰų� �Ҽ���

    public StatData maxStats; // ������ �� �� �� ���� maxStat �����ؼ� ����ϱ�
    public StatData stats;

}

// ��Ʋ�� �Ŵ��� AddUnit�� ��ų�� ������ 