/**********************************************************
* 메인 맵 관리
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    public MainMapDataMaker dataMaker;
    public MainMapMaker mapMaker;
    public MainMapInteraction interaction;

    private void Awake()
    {
        
    }

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

        mapMaker.MakeIcon();

        if (DataManager.instance.nodes.Count == 0)
        {
            Debug.Log($"{GetType()} - 노드 새로만듬");
            mapMaker.MakeNode();
        }
        else
        {
            Debug.Log($"{GetType()} - 있던 노드 씀");
            mapMaker.AddIconToNode();
        }

        mapMaker.DrawLine();
    }
}
