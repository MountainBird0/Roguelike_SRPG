using System.Collections.Generic;
// �Ⱦ��Ű�����
public class UnitInfo
{
    // �������� �����Ҷ� ���� ���ֵ� ���� �޾ƿͼ� ����
    public Dictionary<string, StatData> currentStats = new();
    // ��ų�� ���ְ� ��ų��ȣ?
    public Dictionary<string, List<int>> currentSkills = new();
}
