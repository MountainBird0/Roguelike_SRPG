/**********************************************************
* 아이콘의 종류
***********************************************************/
public enum Icon
{
    MONSTER,
    SHOP,   
    BOSS,
    CHEST
}

public enum IconState
{
    // 못가는(못누름), 이미지나온, 갈수있는(누를수있음)
    LOCKED,
    VISITED,
    ATTAINABLE,
}