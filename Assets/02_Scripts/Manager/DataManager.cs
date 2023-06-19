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

    // default data
    public TextAsset stageLevelText;
    public Dictionary<string, StageLevelData> stageLevels;
    public TextAsset iconProbabilityText;
    public Dictionary<string, IconProbabilityData> iconProbabilitys;

    // playing data
    public GameInfo gameInfo;
    public MapData mapData;

    public List<IconNode> nodes;

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
        iconProbabilitys = new Dictionary<string, IconProbabilityData>();
        gameInfo = new GameInfo();
        mapData = new MapData();
        nodes = new List<IconNode>();
    }

    public void Start()
    {
        LoadDefaultData();
        LoadPlayingData();
    }

    /**********************************************************
    * TextAsset형식으로 되어있는 데이터들 가지고 오기
    ***********************************************************/
    private void LoadDefaultData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
    }
    

    /**********************************************************
    * 이어하기용 json 파일 저장하기
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        SaveTool("MainMapData", mapData);
        SaveTool("GameInfo", gameInfo);
    }


    /**********************************************************
    * 이어하기용 json 파일 불러오기
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("MainMapData", ref mapData) && LoadTool("GameInfo", ref gameInfo);
    }


    /**********************************************************
    * 이어하기용 json 파일 삭제하기
    ***********************************************************/
    public void DeleteSaveData()
    {
        DeleteTool("MainMapData");
        DeleteTool("GameInfo");
        ResetInfo();
    }


    /**********************************************************
    * gameinfo 초기화
    ***********************************************************/
    private void ResetInfo()
    {
        gameInfo.currentStage = 1;
        gameInfo.seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        nodes.Clear();
    }

    /**********************************************************
    * IconState 동기화 : MapData = Nodes
    ***********************************************************/
    public void A()
    {

    }



    /**********************************************************
    * json 저장 툴
    ***********************************************************/
    private void SaveTool<T>(string fileName, T data)
    {
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(data, settings);
        File.WriteAllText(path, setJson);
    }
    JsonSerializerSettings settings = new JsonSerializerSettings // 순환참조 오류 발생했을 때 사용
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };


    /**********************************************************
    * json 로드 툴
    ***********************************************************/
    private bool LoadTool<T>(string fileName, ref T data)
    {
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.Log($"{GetType()} - {fileName} : 못불러옴");
        }
        else
        {
            string json = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<T>(json);          
        }
        return fileInfo.Exists;
    }


    /**********************************************************
    * json 삭제 툴
    ***********************************************************/
    private void DeleteTool(string fileName)
    {
        string path = Application.persistentDataPath + fileName + ".Json";
        File.Delete(path);
    }





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - 저장함");
            SaveDate();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"{GetType()} - 삭제함");
            DeleteSaveData();
        }
    }


    // 끝
}
