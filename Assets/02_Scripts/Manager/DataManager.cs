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

[DefaultExecutionOrder((int)SEO.DataManager)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public TextAsset stageLevelText;
    public Dictionary<string, StageLevelData> stageLevels;

    public TextAsset iconProbabilityText;
    public Dictionary<string, IconProbabilityData> iconProbabilitys;

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

        stageLevels = new Dictionary<string, StageLevelData>();
        stageData = new StageData();
        iconProbabilitys = new Dictionary<string, IconProbabilityData>();
    }

    public void Start()
    {
        LoadData();
    }


    /**********************************************************
    * 새 게임 json 파일 불러오기
    ***********************************************************/
    public void LoadNewData()
    {

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
    * TextAsset형식으로 되어있는 데이터들 가지고 오기
    ***********************************************************/
    private void LoadData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
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

    // 끝
}
