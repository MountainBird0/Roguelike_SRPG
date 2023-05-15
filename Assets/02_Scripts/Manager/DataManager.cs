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
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());

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
