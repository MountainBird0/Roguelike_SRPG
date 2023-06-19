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
    * TextAsset�������� �Ǿ��ִ� �����͵� ������ ����
    ***********************************************************/
    private void LoadDefaultData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
    }
    

    /**********************************************************
    * �̾��ϱ�� json ���� �����ϱ�
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        SaveTool("MainMapData", mapData);
        SaveTool("GameInfo", gameInfo);
    }


    /**********************************************************
    * �̾��ϱ�� json ���� �ҷ�����
    ***********************************************************/
    public bool LoadPlayingData()
    {
        return LoadTool("MainMapData", ref mapData) && LoadTool("GameInfo", ref gameInfo);
    }


    /**********************************************************
    * �̾��ϱ�� json ���� �����ϱ�
    ***********************************************************/
    public void DeleteSaveData()
    {
        DeleteTool("MainMapData");
        DeleteTool("GameInfo");
        ResetInfo();
    }


    /**********************************************************
    * gameinfo �ʱ�ȭ
    ***********************************************************/
    private void ResetInfo()
    {
        gameInfo.currentStage = 1;
        gameInfo.seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        nodes.Clear();
    }

    /**********************************************************
    * IconState ����ȭ : MapData = Nodes
    ***********************************************************/
    public void A()
    {

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





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log($"{GetType()} - ������");
            SaveDate();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log($"{GetType()} - ������");
            DeleteSaveData();
        }
    }


    // ��
}
