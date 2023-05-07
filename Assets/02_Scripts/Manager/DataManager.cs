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
    
    public List<DataMainMap> dataMainMap;
    public LineNum lineNum;

    public TextAsset DefaultlineNum;


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
    }




    /**********************************************************
    * 새 게임 json 파일 불러오기
    ***********************************************************/
    public void LoadNewData()
    {
        LoadLineNumData();
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
        string fileName = "MainMapData";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(dataMainMap);
        File.WriteAllText(path, setJson);
    }

    /**********************************************************
    * Json에서 LineNum 할당된거 가지고 오기
    ***********************************************************/
    private void LoadLineNumData()
    {
        lineNum = JsonConvert.DeserializeObject<LineNum>(DefaultlineNum.ToString());
    }












    /**********************************************************
    * json에 레벨 디자인 저장
    * C:\Users\huy12\AppData\LocalLow\DefaultCompany
    ***********************************************************/
    public void SaveTemp()
    {
        lineNum.stage1Line = 5;
        lineNum.stage2Line = 6;
        lineNum.stage3Line = 7;

        string fileName = "LineNum";
        string path = Application.persistentDataPath + fileName + ".Json";
        var setJson = JsonConvert.SerializeObject(lineNum);
        File.WriteAllText(path, setJson);
    }


}
