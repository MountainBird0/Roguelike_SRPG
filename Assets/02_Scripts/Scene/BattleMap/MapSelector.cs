/**********************************************************
* 배틀 맵에서 불러올 맵을 정함
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public List<GameObject> Stage1Maps;
    public List<GameObject> Stage2Maps;
    public List<GameObject> Stage3Maps;

    /**********************************************************
    * 배틀 맵에서 불러올 맵을 정함
    ***********************************************************/
    public void SelectMap(Transform pos)
    {
        //int currentStage = DataManager.instance.gameInfo.currentStage;
        int currentStage = 1;
        GameObject icon = null;

        switch (currentStage)
        {
            case 1:
                icon = Instantiate(Stage1Maps[Random.Range(0, Stage1Maps.Count)], pos);
                break;
            case 2:
                icon = Instantiate(Stage1Maps[Random.Range(0, Stage2Maps.Count)], pos);
                break;
            case 3:
                icon = Instantiate(Stage1Maps[Random.Range(0, Stage3Maps.Count)], pos);
                break;
        }
    }
}
