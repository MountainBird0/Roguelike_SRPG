/******************************************************************************
* 게임 데이터를 관리
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
    * 새 게임 json 파일 불러오기
    ***********************************************************/
    public void LoadNewData()
    {
        LoadLevelData();
    }


    /**********************************************************
    * 기존 게임 json 파일 불러오기
    ***********************************************************/
    public void LoadSaveData()
    {
        string fileName = "StageData";
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo FileInfo = new FileInfo(path);

        if(!FileInfo.Exists)
        {
            Debug.Log($"{GetType()} - 파일없음");
            return;
        }

        // 저장되어있는 메인 맵 데이터 불러옴
        string json = File.ReadAllText(path);
        stageData = JsonConvert.DeserializeObject<StageData>(json);
    }


    /**********************************************************
    * json에 저장하기
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
    * TextAsset형식으로 되어있는 Leveldata 가지고오기
    ***********************************************************/
    private void LoadLevelData()
    {
        StageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(StageLevelText.ToString());
    }


    /**********************************************************
    * 진행중인 맵 데이터 가지고오기
    ***********************************************************/
    private void GetStageData(StageData currentStageData)
    {
        StageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(StageLevelText.ToString());
    }



    //var a = StageLevels["1"];
    //StageLevelData b;
    //// 가지고온거 활용
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
    * json에 레벨 디자인 저장
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
