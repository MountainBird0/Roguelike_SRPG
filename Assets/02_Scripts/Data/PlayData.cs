using System.Collections.Generic;

public class StageData
{
    public bool isSave;
    public int currentStage;
    public int clearCount;

    public int lineCount;
    public Queue<int> iconCounts;
    public Queue<ICON> iconTypes;
}