/******************************************************************************
* ���� �� ������ ����
*******************************************************************************/
using System.Collections.Generic;

public class DataMainMap
{
    // ������Ƽ ���� ����
    public int stageNum { get; set; }                            // ��������
    public Queue<int> iconNums { get; set; }                     // ���θ��� �� �������� ��
    public List<(int icon, bool check)> iconStates { get; set; } // ������ ������ Ȱ��ȭ ����
}

