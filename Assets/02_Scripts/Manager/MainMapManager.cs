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
        if(GameManager.instance.hasSaveData)
        {
            Debug.Log($"{GetType()} - ����� ������ ����");
        }
        else
        {
            Debug.Log($"{GetType()} - ����� ������ ����");
            dataMaker.MakeMapData();
        }
        // �������� �ϳ� Ŭ���� �ϸ�
        
        
        
        mapMaker.MakeMap();
    }
}
