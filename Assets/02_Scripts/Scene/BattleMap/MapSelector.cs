/**********************************************************
* πË∆≤ ∏ ø°º≠ ∫“∑Øø√ ∏ ¿ª ¡§«‘
***********************************************************/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapSelector : MonoBehaviour
{
    [Header("Stage1")]
    public List<GameObject> stage1Mon;
    public List<GameObject> stage1Elite;
    public List<GameObject> stage1Boss;

    [Header("Stage2")]
    public List<GameObject> stage2Mon;
    public List<GameObject> stage2Elite;
    public List<GameObject> stage2Boss;

    [Header("Stage2")]
    public List<GameObject> stage3Mon;
    public List<GameObject> stage3Elite;
    public List<GameObject> stage3Boss;






    /**********************************************************
    * πË∆≤ ∏ ø°º≠ ∫“∑Øø√ ∏ ¿ª ¡§«‘
    ***********************************************************/
    public GameObject SelectMap(Transform pos)
    {
        var iconType = DataManager.instance.nodes.Last(node => node.iconState == IconState.VISITED).iconInfo.Item1;
        Debug.Log($"{GetType()} - {iconType} ππ¿”");
        //int currentStage = DataManager.instance.gameInfo.currentStage;
        int currentStage = 1;
        GameObject map = null;

        switch (currentStage)
        {
            case 1:
                if(iconType == IconType.MONSTER)
                    map = Instantiate(stage1Mon[Random.Range(0, stage1Mon.Count)], pos);
                else if(iconType == IconType.ELITE)
                    map = Instantiate(stage1Elite[Random.Range(0, stage1Elite.Count)], pos);
                else if (iconType == IconType.BOSS)
                    map = Instantiate(stage1Boss[Random.Range(0, stage1Boss.Count)], pos);
                break;

            case 2:
                if (iconType == IconType.MONSTER)
                    map = Instantiate(stage2Mon[Random.Range(0, stage2Mon.Count)], pos);
                else if (iconType == IconType.ELITE)
                    map = Instantiate(stage2Elite[Random.Range(0, stage2Elite.Count)], pos);
                else if (iconType == IconType.BOSS)
                    map = Instantiate(stage2Boss[Random.Range(0, stage2Boss.Count)], pos);
                break;

            case 3:
                if (iconType == IconType.MONSTER)
                    map = Instantiate(stage3Mon[Random.Range(0, stage3Mon.Count)], pos);
                else if (iconType == IconType.ELITE)
                    map = Instantiate(stage3Elite[Random.Range(0, stage3Elite.Count)], pos);
                else if (iconType == IconType.BOSS)
                    map = Instantiate(stage3Boss[Random.Range(0, stage3Boss.Count)], pos);
                break;
        }

        return map;
    }
}
