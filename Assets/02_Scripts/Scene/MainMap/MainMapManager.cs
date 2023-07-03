/**********************************************************
* 메인 맵 관리
***********************************************************/
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    public MainMapDataMaker dataMaker;
    public MainMapMaker mapMaker;

    private void Start()
    {
        MapGenerate();       
    }

    /**********************************************************
    * 맵 생성하기
    ***********************************************************/
    private void MapGenerate()
    {
        if (!GameManager.instance.hasSaveData)
        {
            Debug.Log($"{GetType()} - 저장된 데이터 없어서 새로만듬");
            dataMaker.MakeMapData();
        }

        if (DataManager.instance.nodes.Count == 0)
        {
            Debug.Log($"{GetType()} - 노드 새로만듬");
            mapMaker.MakeNode();
        }
        mapMaker.MakeIcon();
        mapMaker.DrawLine();
    }
}
