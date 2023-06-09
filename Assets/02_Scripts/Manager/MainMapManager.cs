/**********************************************************
* ���� ���� ����
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
            Debug.Log($"{GetType()} - ����� ������ ��� ���θ���");
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
