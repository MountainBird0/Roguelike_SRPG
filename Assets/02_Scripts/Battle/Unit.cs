/**********************************************************
* �� ������ ���� ����
***********************************************************/
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public Sprite smallIcon;
    public Sprite BigIcon;

    // ���� ���� �� ��

    public TileLogic tile; // �̰� �׳� vec3�� �ٲٰų� �Ҽ���

    public StatData maxStats; // ������ �� �� �� ���� maxStat �����ؼ� ����ϱ�
    public StatData stats;

    public PlayerType playerType;
    public AllianceType allianceType;
    // ���� �߰�
    public int faction;
}

