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

    public TextAsset stageLevelText;
    public Dictionary<string, StageLevelData> stageLevels;


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

        // lineNums = new List<LineNum>();
        stageLevels = new Dictionary<string, StageLevelData>();
    }




    /**********************************************************
    * �� ���� json ���� �ҷ�����
    ***********************************************************/
    public void LoadNewData()
    {
        LoadLevelData();
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

        //string fileName = "MainMapData";
        //string path = Application.persistentDataPath + fileName + ".Json";
        //var setJson = JsonConvert.SerializeObject(dataMainMaps);
        //File.WriteAllText(path, setJson);
    }

    /**********************************************************
    * TextAsset�������� �Ǿ��ִ� Leveldata ���������
    ***********************************************************/
    private void LoadLevelData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());

    }



    //var a = StageLevels["1"];
    //StageLevelData b;
    //// ������°� Ȱ��
    //Debug.Log($"{GetType()} - {a.lineNum}");

    //if(StageLevels.TryGetValue("2", out b))
    //{
    //    Debug.Log($"{GetType()} - {b.lineNum}");
    //    Debug.Log($"{GetType()} - {b.monsterPer}");
    //    Debug.Log($"{GetType()} - {b.shopNum}");
    //    Debug.Log($"{GetType()} - {b.stage}");
    //    Debug.Log($"{GetType()} - {b.lastLine}");
    //    Debug.Log($"{GetType()} - {b.monsterPer}");
    //}



    //lineNums = JsonConvert.DeserializeObject<LineNum>(DefaultlineNum.ToString());








    /**********************************************************
    * json�� ���� ������ ����
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveTemp()
    {
        //lineNum.line = 111;
        //lineNums.Add(lineNum);
        //lineNum.line = 222;
        //lineNums.Add(lineNum);
        //lineNum.line = 333;
        //lineNums.Add(lineNum);


        //string fileName = "LineNum";
        //string path = Application.persistentDataPath + fileName + ".Json";
        //var setJson = JsonConvert.SerializeObject(lineNums);
        //File.WriteAllText(path, setJson);
    }


}
