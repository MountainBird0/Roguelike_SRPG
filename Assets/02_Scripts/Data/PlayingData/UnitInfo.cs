using System.Collections.Generic;

public class UnitInfo
{
    // 스테이지 종료할때 현재 유닛들 상태 받아와서 갱신
    public Dictionary<string, StatData> currentStats = new();
    // 스킬은 유닛과 스킬번호?
    public Dictionary<string, List<string>> currentSkills = new();
}
