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

        mapMaker.MakeIcon();

        if (DataManager.instance.nodes.Count == 0)
        {
            Debug.Log($"{GetType()} - ��� ���θ���");
            mapMaker.MakeNode();
        }
        else
        {
            Debug.Log($"{GetType()} - �ִ� ��� ��");
            mapMaker.AddIconToNode();
        }

        mapMaker.DrawLine();
    }
}
