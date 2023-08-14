/******************************************************************************
* 게임 데이터를 관리
*******************************************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.DataManager)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // default data
    // 앞에 두개 나중에 int로
    public TextAsset stageLevelText;
    public Dictionary<string, StageLevelData> stageLevels = new();
    public TextAsset iconProbabilityText;
    public Dictionary<string, IconProbabilityData> iconProbabilitys = new();
    public TextAsset UnitStatsText;
    public Dictionary<string, StatData> defaultUnitStats = new();
    public TextAsset SkillsText;
    public Dictionary<int, SkillData> defaultSkills = new();
    public TextAsset UnitAndSkillText;
    public Dictionary<string, SkillListData> defaultUnitSkills = new();

    // playing data
    public GameInfo gameInfo;
    public MapInfo mapInfo = new();
    
    public Dictionary<string, StatData> currentUnitInfo = new();
    public Dictionary<string, SkillListData> currentUnitSkills = new();

    public List<IconNode> nodes = new();

    private void Awake()
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
    * TextAsset형식으로 되어있는 데이터들 가지고 오기
    ***********************************************************/
    public void LoadDefaultData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
        defaultUnitStats = JsonConvert.DeserializeObject<Dictionary<string, StatData>>(UnitStatsText.ToString());
        defaultSkills = JsonConvert.DeserializeObject<Dictionary<int, SkillData>>(SkillsText.ToString());
        defaultUnitSkills = JsonConvert.DeserializeObject<Dictionary<string, SkillListData>>(UnitAndSkillText.ToString());

        // 이거 이어하기때는 안해도 ㄱㅊ
        currentUnitInfo = defaultUnitStats;
        currentUnitSkills = defaultUnitSkills;

        //foreach (var a in currentUnitSkills)
        //{
        //    Debug.Log($"{GetType()} - {a.Key}");
        //    Debug.Log($"{GetType()} - {a.Value.list.Count}");
        //    foreach (var b in a.Value.list)
        //    {
        //        Debug.Log($"{GetType()} - {b}");
        //    }
        //}
    }
    

    /**********************************************************
    * 이어하기용 json 파일 저장하기
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        SaveTool("GameInfo", gameInfo);
        SaveTool("MainMapInfo", mapInfo);
        SaveTool("UnitInfo", currentUnitInfo);
        SaveTool("UnitSkillInfo", currentUnitSkills);
    }


    /**********************************************************
    * 이어하기용 json 파일 불러오기
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("GameInfo", ref gameInfo) && 
            LoadTool("MainMapInfo", ref mapInfo) &&             
            LoadTool("UnitInfo", ref currentUnitInfo) &&
            LoadTool("UnitSkillInfo", ref currentUnitSkills);


    }


    /**********************************************************
    * 이어하기용 json 파일 삭제하기
    ***********************************************************/
    public void DeleteSaveData()
    {
        DeleteTool("GameInfo");
        DeleteTool("MainMapInfo");
        DeleteTool("UnitInfo");
        DeleteTool("UnitSkillInfo");
        ResetGameInfo();
    }


    /**********************************************************
    * gameinfo 초기화
    ***********************************************************/
    private void ResetGameInfo()
    {
        gameInfo.currentStage = 1;
        gameInfo.seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        nodes.Clear();
    }


    /**********************************************************
    * IconState 동기화 : MapData = Nodes
    ***********************************************************/
    public void OverWriteState()
    {
        for (int i = 0; i < mapInfo.iconStates.Count; i++)
        {
            mapInfo.iconStates[i] = nodes[i + 1].iconState;
        }
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
}
