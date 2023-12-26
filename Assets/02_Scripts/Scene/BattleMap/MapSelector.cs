/**********************************************************
* ��Ʋ �ʿ��� �ҷ��� ���� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public List<GameObject> Stage1Maps;
    public List<GameObject> Stage2Maps;
    public List<GameObject> Stage3Maps;

    /**********************************************************
    * ��Ʋ �ʿ��� �ҷ��� ���� ����
    ***********************************************************/
    public GameObject SelectMap(Transform pos)
    {
        //int currentStage = DataManager.instance.gameInfo.currentStage;
        int currentStage = 1;
        GameObject map = null;

        switch (currentStage)
        {
            case 1:
                map = Instantiate(Stage1Maps[Random.Range(0, Stage1Maps.Count)], pos);
                break;
            case 2:
                map = Instantiate(Stage1Maps[Random.Range(0, Stage2Maps.Count)], pos);
                break;
            case 3:
                map = Instantiate(Stage1Maps[Random.Range(0, Stage3Maps.Count)], pos);
                break;
        }

        return map;
    }
}
