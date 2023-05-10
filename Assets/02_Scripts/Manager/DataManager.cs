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

    public TextAsset StageLevelText;
    public Dictionary<string, StageLevelData> StageLevels;

    public StageData stageData;

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

        StageLevels = new Dictionary<string, StageLevelData>();
        stageData = new StageData()
        {
            iconPerLines = new Queue<int>(),
            iconIndexs = new Queue<int>(),
        };
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
        string fileName = "StageData";
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo FileInfo = new FileInfo(path);

        if(!FileInfo.Exists)
        {
            Debug.Log($"{GetType()} - ���Ͼ���");
            return;
        }

        // ����Ǿ��ִ� ���� �� ������ �ҷ���
        string json = File.ReadAllText(path);
        stageData = JsonConvert.DeserializeObject<StageData>(json);
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
        StageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(StageLevelText.ToString());
    }


    /**********************************************************
    * �������� �� ������ ���������
    ***********************************************************/
    private void GetStageData(StageData currentStageData)
    {
        StageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(StageLevelText.ToString());
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
