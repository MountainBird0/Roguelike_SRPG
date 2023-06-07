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
        // ���̺� �����Ͱ� ������
        if(DataManager.instance.mapData.currentStage == 0)
        {
            Debug.Log($"{GetType()} - ����� ������ ����");
            dataMaker.MakeMapData();
            mapMaker.MakeMap();
        }
        else
        {
            Debug.Log($"{GetType()} - ����� ������ ����");
            mapMaker.MakeMap();
        }

        // ���̺� �����Ͱ� ������

        // �������� �ϳ� Ŭ���� �ϸ�
    }

}
