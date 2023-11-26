/**********************************************************
* 각 스킬의 정보 저장
***********************************************************/
public struct SkillData
{
    public string name;
    public string explain;

    public int range;
    public int AOERange;

    public AffectType affectType;

    public int jobType;
    public RangeType rangeType;
    public DamageType damageType;

    public float multiplier;

    public int currentCoolTime;
    public int coolTime;

    public bool isDirectional;
    public bool isAOE;
}
