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
    
    public List<DataMainMap> dataMainMaps;
    public List<LineNum> lineNums;
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

        lineNums = new List<LineNum>();
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
        var setJson = JsonConvert.SerializeObject(dataMainMaps);
        File.WriteAllText(path, setJson);
    }

    /**********************************************************
    * Json���� LineNum �Ҵ�Ȱ� ������ ����
    ***********************************************************/
    private void LoadLineNumData()
    {
        //lineNums = JsonConvert.DeserializeObject<LineNum>(DefaultlineNum.ToString());
    }












    /**********************************************************
    * json�� ���� ������ ����
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveTemp()
    {
        lineNum.line = 111;
        lineNums.Add(lineNum);
        lineNum.line = 222;
        lineNums.Add(lineNum);
        lineNum.line = 333;
        lineNums.Add(lineNum);


        string fileName = "LineNum";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(lineNums);
        File.WriteAllText(path, setJson);
    }


}
