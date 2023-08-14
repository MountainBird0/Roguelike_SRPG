/******************************************************************************
* ���� �����͸� ����
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
    // �տ� �ΰ� ���߿� int��
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
    * TextAsset�������� �Ǿ��ִ� �����͵� ������ ����
    ***********************************************************/
    public void LoadDefaultData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
        defaultUnitStats = JsonConvert.DeserializeObject<Dictionary<string, StatData>>(UnitStatsText.ToString());
        defaultSkills = JsonConvert.DeserializeObject<Dictionary<int, SkillData>>(SkillsText.ToString());
        defaultUnitSkills = JsonConvert.DeserializeObject<Dictionary<string, SkillListData>>(UnitAndSkillText.ToString());

        // �̰� �̾��ϱ⶧�� ���ص� ����
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
    * �̾��ϱ�� json ���� �����ϱ�
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
    * �̾��ϱ�� json ���� �ҷ�����
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("GameInfo", ref gameInfo) && 
            LoadTool("MainMapInfo", ref mapInfo) &&             
            LoadTool("UnitInfo", ref currentUnitInfo) &&
            LoadTool("UnitSkillInfo", ref currentUnitSkills);


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
