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

    public bool hasSaveData;

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
        LoadSaveData();
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
    * json에 저장하기
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(mapData, settings);
        File.WriteAllText(path, setJson);
    }
    JsonSerializerSettings settings = new JsonSerializerSettings // 순환참조 오류 발생했을 때 사용
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };


    /**********************************************************
    * 기존 게임 json 파일 불러오기
    ***********************************************************/
    public void LoadSaveData()
    {
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.Log($"{GetType()} - 저장된 데이터 없음");
            
            hasSaveData = false;
        }
        else
        {
            Debug.Log($"{GetType()} - 저장된거 불러옴");
            
            string json = File.ReadAllText(path);
            mapData = JsonConvert.DeserializeObject<MapData>(json);
            
            hasSaveData = true;
        }
    }


    /**********************************************************
    * 기존 게임 json 파일 삭제하기
    ***********************************************************/
    public void DeleteSaveData()
    {
        string fileName = "MainMapData";
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
