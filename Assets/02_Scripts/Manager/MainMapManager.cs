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
        if(!GameManager.instance.hasSaveData)
        {
            Debug.Log($"{GetType()} - 저장된 데이터 없어서 새로만듬");
            dataMaker.MakeMapData();
        }
        
        mapMaker.MakeMap();

        if(true) // 
        {
            mapMaker.MakeNode();
        }
        else
        {
            mapMaker.AddIconToNode();
        }  
    }
}
