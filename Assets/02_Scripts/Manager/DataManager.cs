/******************************************************************************
* ���� �����͸� ����
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
    // �տ� �ΰ� ���߿� int��
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
    * TextAsset�������� �Ǿ��ִ� �����͵� ������ ����
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

        // �̰� �̾��ϱ⶧�� ���ص� ���� -> ���� �����ϴ°� �߰��Ҷ� ���� ����
        currentUnitStats = defaultUnitStats;

        // ���� ������ ��� ������ ��ų ��Ͽ��� �ִ� 3������ ���� -> ���� �����ϴ°� �߰��Ҷ� ���� ����
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
    * �̾��ϱ�� json ���� �����ϱ�
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
    * �̾��ϱ�� json ���� �ҷ�����
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("GameInfo", ref gameInfo) && 
            LoadTool("MainMapInfo", ref mapInfo) &&             
            LoadTool("UnitInfo", ref currentUnitStats) &&
            LoadTool("UnitSkillInfo", ref currentUsableSkills);
    }

    /**********************************************************
    * �̾��ϱ�� json ���� �����ϱ�
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
    * gameinfo �ʱ�ȭ
    ***********************************************************/
    private void ResetGameInfo()
    {
        gameInfo.currentStage = 1;
        gameInfo.seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        nodes.Clear();
    }

    /**********************************************************
    * IconState ����ȭ : MapData = Nodes
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

        // ���ֺ��� ������ ���� ��
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
    * json ���� ��
    ***********************************************************/
    private void SaveTool<T>(string fileName, T data)
    {
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(data, settings);
        File.WriteAllText(path, setJson);
    }
    JsonSerializerSettings settings = new JsonSerializerSettings // ��ȯ���� ���� �߻����� �� ���
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    /**********************************************************
    * json �ε� ��
    ***********************************************************/
    private bool LoadTool<T>(string fileName, ref T data)
    {
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.Log($"{GetType()} - {fileName} : ���ҷ���");
        }
        else
        {
            string json = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<T>(json);          
        }
        return fileInfo.Exists;
    }

    /**********************************************************
    * json ���� ��
    ***********************************************************/
    private void DeleteTool(string fileName)
    {
        string path = Application.persistentDataPath + fileName + ".Json";
        File.Delete(path);
    }
}
