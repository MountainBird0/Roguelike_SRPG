/**********************************************************
* ���� �� ����
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
    * �� �����ϱ�
    ***********************************************************/
    private void MapGenerate()
    {
        if (!GameManager.instance.hasSaveData)
        {
            Debug.Log($"{GetType()} - ����� ������ ��� ���θ���");
            dataMaker.MakeMapData();
        }

        // mapMaker.MakeIconTT();

        if (DataManager.instance.nodes.Count == 0)
        {
            Debug.Log($"{GetType()} - ��� ���θ���");
            mapMaker.MakeNode();
        }
        mapMaker.MakeIcon();
        mapMaker.DrawLine();
    }
}
