/**********************************************************
* 메인 맵을 관리
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapManager : MonoBehaviour
{
    public MainMapDataMaker dataMaker;
    public MainMapMaker mapMaker;

    private void Start()
    {
        MapGenerate();       
    }

    private void MapGenerate()
    {
        // 세이브 데이터가 없으면
        if(DataManager.instance.mapData.currentStage == 0)
        {
            Debug.Log($"{GetType()} - 저장된 데이터 없음");
            dataMaker.MakeMapData();
            mapMaker.MakeMap();
        }
        else
        {
            Debug.Log($"{GetType()} - 저장된 데이터 있음");
            mapMaker.MakeMap();
        }

        // 세이브 데이터가 있으면

        // 스테이지 하나 클리어 하면
    }

}
