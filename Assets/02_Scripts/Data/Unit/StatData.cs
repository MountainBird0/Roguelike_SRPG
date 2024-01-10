/**********************************************************
* ∞¢ ¿Ø¥÷∫∞ Ω∫≈» ¡§∫∏ ¿˙¿Â
***********************************************************/
public struct StatData
{
    public int Level;
    public int MaxHP;
    public int HP;
    public int ATK;
    public int DEF;
    public int MATK;
    public int MDEF;
    public int HIT;
    public int EVA;
    public int CRI;
    public int RES;
    public int MOV;
    public int SPEED;
    public int CurEXP;
    public int MaxEXP;
    public int dropEXP;
    public int jobType;

    public StatData IncreaseLevel(StatGrowData growData)
    {
        Level++;

        MaxHP += growData.HpGrow;
        HP = MaxHP;

        ATK += growData.ATKGrow;
        DEF += growData.DEFGrow;
        MATK += growData.MATKGrow;
        MDEF += growData.MDEFGrow;

        CurEXP = 0;
        MaxEXP += growData.reqEXPGrow;

        return this;
    }

    public StatData HpFullUp()
    {
        HP = MaxHP;
        return this;
    }
}
