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

    // public int icon { get; set; }
    // public byte check { get; set; }
}

//public class icon
//{
//    int ����;
//    int ���� ���������
//    bool Ȱ��ȭ;
//}



