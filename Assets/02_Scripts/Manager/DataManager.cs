/******************************************************************************
* 게임 데이터를 관리
*******************************************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.DataManager)]
public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    // default data
    // 앞에 두개 나중에 int로
    [Header("stage")]
    public TextAsset stageLevelText;
    public Dictionary<string, StageLevelData> stageLevels = new();
    public TextAsset iconProbabilityText;
    public Dictionary<string, IconProbabilityData> iconProbabilitys = new();

    [Header("stat")]
    public TextAsset unitStatsText;
    public Dictionary<string, StatData> defaultUnitStats = new();
    public TextAsset unitGrowText;
    public Dictionary<string, StatGrowData> defaultUnitGrowStats = new();
    public TextAsset monsterStatsText;
    public Dictionary<string, StatData> defaultMonsterStats = new();
    public TextAsset monsterGrowText;
    public Dictionary<string, StatGrowData> defaultMonsterGrowStats = new();

    [Header("skill")]
    public TextAsset skillStatsText;
    public Dictionary<int, SkillData> defaultSkillStats = new();
    public TextAsset usableSkillsText;
    public Dictionary<string, List<int>> defaultUsableSkills = new();
    public TextAsset monsterSkillsText;
    public Dictionary<string, List<int>> defaultMonsterEquipSkills = new();


    // playing data
    public GameInfo gameInfo;
    public MapInfo mapInfo = new();
    
    public Dictionary<string, StatData> currentUnitStats = new();
    public Dictionary<string, List<int>> currentUsableSkills = new();
    public Dictionary<string, List<int>> currentShopSkills = new();
    public Dictionary<string, List<int>> currentEquipSkills = new();

    public List<int> shopSkills = new();

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

        defaultUnitStats = JsonConvert.DeserializeObject<Dictionary<string, StatData>>(unitStatsText.ToString());
        defaultUnitGrowStats = JsonConvert.DeserializeObject<Dictionary<string, StatGrowData>>(unitGrowText.ToString());
        defaultMonsterStats = JsonConvert.DeserializeObject<Dictionary<string, StatData>>(monsterStatsText.ToString());
        defaultMonsterGrowStats = JsonConvert.DeserializeObject<Dictionary<string, StatGrowData>>(monsterGrowText.ToString());

        defaultSkillStats = JsonConvert.DeserializeObject<Dictionary<int, SkillData>>(skillStatsText.ToString());
        defaultUsableSkills = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(usableSkillsText.ToString());
        defaultMonsterEquipSkills = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(monsterSkillsText.ToString());

        // 이거 이어하기때는 안해도 ㄱㅊ -> 게임 저장하는거 추가할때 같이 수정
        currentUnitStats = defaultUnitStats;

        // 현재 유닛이 사용 가능한 스킬 목록에서 최대 3개까지 장착 -> 게임 저장하는거 추가할때 같이 수정
        currentUsableSkills = defaultUsableSkills;
        foreach (var kvp in currentUsableSkills)
        {
            List<int> numList = new();

            for(int i = 0; i < 3; i++)
            {
                if( i < kvp.Value.Count)
                {
                    numList.Add(kvp.Value[i]);
                }
            }
            currentEquipSkills.Add(kvp.Key, numList);
        }

        SettingShopSkill();


        //foreach (var a in currentSlotSkills)
        //{
        //    Debug.Log($"{GetType()} - {a.Key}");
        //    Debug.Log($"{GetType()} - {a.Value.list}");
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
        SaveTool("UnitInfo", currentUnitStats);
        SaveTool("UnitSkillInfo", currentUsableSkills);
    }

    /**********************************************************
    * 이어하기용 json 파일 불러오기
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("GameInfo", ref gameInfo) && 
            LoadTool("MainMapInfo", ref mapInfo) &&             
            LoadTool("UnitInfo", ref currentUnitStats) &&
            LoadTool("UnitSkillInfo", ref currentUsableSkills);
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

    private void SettingShopSkill()
    {
        var usableSkills = currentUsableSkills.SelectMany(kvp => kvp.Value);

        shopSkills = defaultSkillStats.Keys.Except(usableSkills).ToList();

        // 유닛별로 나누고 싶을 때
        //foreach(var kvp in defaultSkillStats)
        //{
        //    int jobType = kvp.Value.jobType;
        //    string unitName = defaultUnitStats.FirstOrDefault(entry => entry.Value.jobType == jobType).Key;

        //    if (unitName == null) continue;

        //    List<int> skills = currentUsableSkills[unitName];

        //    if (!skills.Contains(kvp.Key))
        //    {
        //        if (!currentShopSkills.ContainsKey(unitName))
        //        {
        //            currentShopSkills[unitName] = new List<int>();
        //        }
        //        currentShopSkills[unitName].Add(kvp.Key);
        //    }
        //}
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
