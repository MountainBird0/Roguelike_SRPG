/******************************************************************************
* ���� �����͸� ����
*******************************************************************************/
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    public List<DataMainMap> dataMainMap;
    public LineNum lineNum;

    public TextAsset DefaultlineNum;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }




    /**********************************************************
    * �� ���� json ���� �ҷ�����
    ***********************************************************/
    public void LoadNewData()
    {
        LoadLineNumData();
    }

    /**********************************************************
    * ���� ���� json ���� �ҷ�����
    ***********************************************************/
    public void LoadSaveData()
    {
        
    }


    /**********************************************************
    * json�� �����ϱ�
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(dataMainMap);
        File.WriteAllText(path, setJson);
    }

    /**********************************************************
    * Json���� LineNum �Ҵ�Ȱ� ������ ����
    ***********************************************************/
    private void LoadLineNumData()
    {
        lineNum = JsonConvert.DeserializeObject<LineNum>(DefaultlineNum.ToString());
    }












    /**********************************************************
    * json�� ���� ������ ����
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveTemp()
    {
        lineNum.stage1Line = 5;
        lineNum.stage2Line = 6;
        lineNum.stage3Line = 7;

        string fileName = "LineNum";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(lineNum);
        File.WriteAllText(path, setJson);
    }


}
