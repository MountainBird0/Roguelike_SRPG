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
    * TextAsset�������� �Ǿ��ִ� �����͵� ������ ����
    ***********************************************************/
    private void LoadDefaultData()
    {
        stageLevels = JsonConvert.DeserializeObject<Dictionary<string, StageLevelData>>(stageLevelText.ToString());
        iconProbabilitys = JsonConvert.DeserializeObject<Dictionary<string, IconProbabilityData>>(iconProbabilityText.ToString());
    }
    

    /**********************************************************
    * json�� �����ϱ�
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveDate()
    {
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(mapData, settings);
        File.WriteAllText(path, setJson);
    }
    JsonSerializerSettings settings = new JsonSerializerSettings // ��ȯ���� ���� �߻����� �� ���
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };


    /**********************************************************
    * ���� ���� json ���� �ҷ�����
    ***********************************************************/
    public void LoadSaveData()
    {
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";

        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
        {
            Debug.Log($"{GetType()} - ����� ������ ����");
            
            hasSaveData = false;
        }
        else
        {
            Debug.Log($"{GetType()} - ����Ȱ� �ҷ���");
            
            string json = File.ReadAllText(path);
            mapData = JsonConvert.DeserializeObject<MapData>(json);
            
            hasSaveData = true;
        }
    }


    /**********************************************************
    * ���� ���� json ���� �����ϱ�
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
