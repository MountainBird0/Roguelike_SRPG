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
        if(GameManager.instance.hasSaveData)
        {
            Debug.Log($"{GetType()} - 저장된 데이터 있음");
        }
        else
        {
            Debug.Log($"{GetType()} - 저장된 데이터 없음");
            dataMaker.MakeMapData();
        }
        // 스테이지 하나 클리어 하면
        
        
        
        mapMaker.MakeMap();
    }
}
