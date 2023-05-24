/**********************************************************
* 메인 맵 관련 데이터를 생성함
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder((int)SEO.MainMapDataMaker)]
public class MainMapDataMaker : MonoBehaviour
{
    private List<StageLevelData> stageLevels;
    private StageLevelData stageLevel;
    private IconProbabilityData iconProbability;
    private Dictionary<ICON, int> probabilityMap;
    
    private StageDataTempA stageDataTempA;
    private List<MapData> mapdatas;

    private List<IconData> iconDatas;
    private List<Tree> iconTrees;

    private int currentStage;

    private float mapWidth;
    private float mapHeight;

    private int currentShopCount;


    private void Awake()
    {
        stageLevel = new StageLevelData();
        iconProbability = new IconProbabilityData();
        probabilityMap = new Dictionary<ICON, int>();
        
        stageDataTempA = new StageDataTempA();

        stageLevels = new List<StageLevelData>();
        mapdatas = new List<MapData>();
    }

    private void Start()
    {
        if(GameManager.instance.isNewGame)
        {
            MakeMapData();
        }
        // 새로하기 의미
        if (!stageDataTempA.isSave)
        {
            Debug.Log($"{GetType()} - 새로운 맵 데이터 생성");
            //MakeMapDataTempA();
        }
    }

    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    ***********************************************************/
    private void MakeMapData()
    {
        SetData();

        float mapWidth;
        float mapHeight;

        float widthGap;

        Vector2 iconPos = Vector2.zero;

        // 각 레벨
        for (int i = 0; i < stageLevels.Count; i++)
        {
            AddProbabilityDic(i + 1);

            mapdatas[i].lineCount = stageLevels[i].lineCount;

            mapWidth = stageLevels[i].mapWidth;
            mapHeight = stageLevels[i].mapHeight;
           
            iconPos.x = -(mapWidth / 2); // 아이콘의 초기 x좌표 값 (가장 아래)
            widthGap = mapWidth / (mapdatas[i].lineCount - 1);
            
            // 한 레벨의 각 라인
            for (int j = 0; j < stageLevels[i].lineCount; j++)
            {
                // 첫번째 라인
                if(j == 0)
                {
                    SetIcon(i, mapHeight, stageLevels[i].firstLine, ICON.MONSTER, ref iconPos);
                }
                // 마지막 라인
                else if(j == (stageLevels[i].lineCount -1))
                {
                    SetIcon(i, mapHeight, stageLevels[i].lastLine, ICON.BOSS, ref iconPos);
                }
                else
                {
                    // 상자 라인
                    if (j == (stageLevels[i].chestLine - 1))
                    {
                        SetIcon(i, mapHeight, Random.Range(2, 6), ICON.CHEST, ref iconPos);
                    }
                    else
                    {
                        SetIcon(i, mapHeight, Random.Range(2, 6), ref iconPos);
                    }
                }
                iconPos.x += widthGap;
            }
        }

        DataManager.instance.mapdatas = mapdatas;
    }


    /**********************************************************
    * 고정 아이콘 데이터 넣기
    ***********************************************************/
    public void SetIcon(int currentLevel, float mapHeight, int iconCount, ICON icon, ref Vector2 pos)
    {
        mapdatas[currentLevel].iconCounts.Add(iconCount);

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            mapdatas[currentLevel].iconState.Add((icon, pos));
            pos.y += heightGap;
        }
    }
    /**********************************************************
    * 랜덤 아이콘 데이터 넣기
    ***********************************************************/
    public void SetIcon(int currentLevel, float mapHeight, int iconCount, ref Vector2 pos)
    {
        mapdatas[currentLevel].iconCounts.Add(iconCount);

        float heightGap = mapHeight / (iconCount + 1);
        pos.y = -(mapHeight / 2) + heightGap;

        for (int i = 0; i < iconCount; i++)
        {
            int randomValue = Random.Range(0, GetTotalProbability());

            int sum = 0;

            foreach (var kvp in probabilityMap)
            {
                sum += kvp.Value;

                if (randomValue < sum)
                {
                    mapdatas[currentLevel].iconState.Add((kvp.Key, pos));
                    pos.y += heightGap;
                    ProbabilityCheck(kvp.Key, currentLevel);
                    break;
                }
            }
        }
    }

    /**********************************************************
    * 랜덤 아이콘 dic관리
    ***********************************************************/
    private void ProbabilityCheck(ICON icon, int currentLevel)
    {
        switch (icon)
        {
            case ICON.MONSTER:
                if (probabilityMap.ContainsKey(ICON.SHOP))
                {
                    probabilityMap[ICON.SHOP] += 10;
                }
                break;

            case ICON.SHOP:
                currentShopCount += 1;
                probabilityMap[ICON.SHOP] = iconProbability.shopChance;
                if (currentShopCount == stageLevels[currentLevel].shopCount)
                {
                    probabilityMap.Remove(ICON.SHOP);
                }
                break;
        }
    }

    /**********************************************************
    * 랜덤 아이콘 dic 추가
    ***********************************************************/
    private void AddProbabilityDic(int currentStage)
    {
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        probabilityMap.Clear();
        currentShopCount = 0;

        probabilityMap.Add(ICON.SHOP, iconProbability.shopChance);
        probabilityMap.Add(ICON.MONSTER, iconProbability.monsterChance);
    }


    /**********************************************************
    * 랜덤 아이콘의 전체 확률 구하기
    ***********************************************************/
    private int GetTotalProbability()
    {
        int totalProbability = 0;
        foreach (var kvp in probabilityMap)
        {
        Debug.Log($"{GetType()} - {kvp}");
            totalProbability += kvp.Value;
        }
        return totalProbability;
    }

    /**********************************************************
    * 맵 생성을 위한 default데이터 준비
    ***********************************************************/
    private void SetData()
    {
        // 시드 생성
        int seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        Random.InitState(seed);
        //DataManager.instance.gameInfo.seed = seed;

        for(int i = 1; i < DataManager.instance.stageLevels.Count + 1; i++)
        {
            stageLevels.Add(DataManager.instance.stageLevels[i.ToString()]);
        }

        for (int i = 0; i < stageLevels.Count; i++)
        {
            MapData mapData = new MapData();
            mapdatas.Add(mapData);
        }
    }

    /**********************************************************
    * 생성한 아이콘에 좌표값과 어느 부모 노드에 들어갈지 입히기
    ***********************************************************/
    private void SetIconGrid()
    {

    }




    /**********************************************************
    * 새로운 맵 생성을 위한 데이터 만듬
    * - 게임을 처음 시작할 때, 한 stage를 클리어 했을 때 사용
    * - 상점 가중확률 추가 필요
    ***********************************************************/
    public void MakeMapDataTempA()
    {
        SetDataTempA();

        int iconCount;

        int currentShopCount = 0;

        Vector2 iconPos;
        iconPos.x = -(mapWidth / 2);
        float widthGap = mapWidth / (stageLevel.lineCount - 1);
        float heightGap;

        for (int i = 0; i < stageLevel.lineCount; i++)
        {
            // 첫번째 라인
            if (i == 0) 
            {
                stageDataTempA.iconCounts.Enqueue(stageLevel.firstLine);

                heightGap = mapHeight / (stageLevel.firstLine + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;

                for (int j = 0; j < stageLevel.firstLine; j++)
                {
                    stageDataTempA.iconPos.Enqueue(iconPos);
                    iconPos.y += heightGap;
                    stageDataTempA.iconTypes.Enqueue(ICON.MONSTER);
                }
            }
            // 마지막 라인
            else if (i == (stageLevel.lineCount - 1))
            {
                stageDataTempA.iconCounts.Enqueue(stageLevel.lastLine);

                heightGap = mapHeight / (stageLevel.lastLine + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;

                for (int j = 0; j < stageLevel.lastLine; j++)
                {
                    stageDataTempA.iconPos.Enqueue(iconPos);
                    iconPos.y += heightGap;
                    stageDataTempA.iconTypes.Enqueue(ICON.BOSS);
                }
            }
            else
            {
                iconCount = Random.Range(2, 6);
                stageDataTempA.iconCounts.Enqueue(iconCount);

                heightGap = mapHeight / (iconCount + 1);
                iconPos.y = -(mapHeight / 2) + heightGap;
                // 상자 라인
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageDataTempA.iconPos.Enqueue(iconPos);
                        iconPos.y += heightGap;
                        stageDataTempA.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // 일반 라인
                else
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        int randomValue = Random.Range(0, GetTotalProbability());
                        int sum = 0;
                        foreach (var kvp in probabilityMap)
                        {
                            sum += kvp.Value;
                            if (randomValue < sum)
                            {
                                stageDataTempA.iconPos.Enqueue(iconPos);
                                iconPos.y += heightGap;
                                stageDataTempA.iconTypes.Enqueue(kvp.Key);
                                switch(kvp.Key)
                                {
                                    case ICON.MONSTER:
                                        if(probabilityMap.ContainsKey(ICON.SHOP))
                                        {
                                            probabilityMap[ICON.SHOP] += 10;
                                        }
                                        break;

                                    case ICON.SHOP:
                                        currentShopCount += 1;
                                        probabilityMap[ICON.SHOP] = iconProbability.shopChance;
                                        if (currentShopCount == stageLevel.shopCount)
                                        {
                                            probabilityMap.Remove(ICON.SHOP);
                                        }
                                        break;
                                }
                                break;
                            }
                        }
                    }
                }
            }
            iconPos.x += widthGap;
        }
            stageDataTempA.isSave = true;
            DataManager.instance.stageDataTempA = stageDataTempA;
    }

    /**********************************************************
    * icon끼리 연결을 위한 노드 생성
    ***********************************************************/
    private void MakeTreeData()
    {
        stageDataTempA.iconGrid[0, 1][0] = 2;


        //iconTrees = new List<Tree>(stageLevel.firstLine);
        //iconTrees[0].AddNode(iconDatas[0], iconDatas[1]);

        int preIconCount = 0;
        int iconCount;
        int countGap;

        for (int i = 0; i < stageDataTempA.lineCount; i++)
        {
            iconCount = stageDataTempA.iconCounts.Dequeue();

            for (int j = 0; j < iconCount; j++) // 한 라인 안의 아이콘
            {
                
                if( i == 0)
                {
                    stageDataTempA.iconGrid[i, j][0] = 0;
                }
                else
                {
                    countGap = iconCount - preIconCount;
                }
            }


            preIconCount = iconCount;
        }

        //노드 관련?
        //if (j == 0)
        //{
        //    // 다음 j = 0과 연결
        //}
        //else if (j == stageLevel.firstLine - 1)
        //{
        //    // 다음 j = stageLevel.firstLine - 1과 연결
        //}
        //else
        //{

        //}

    }

    /**********************************************************
    * 맵 생성을 위한 데이터 갱신
    ***********************************************************/
    private void SetDataTempA()
    {
        int seed = (System.DateTime.Now.Millisecond + 1) * (System.DateTime.Now.Second + 1) * (System.DateTime.Now.Minute + 1);
        Random.InitState(seed);

        currentStage = GameManager.instance.currentStage;
        stageLevel = DataManager.instance.stageLevels[currentStage.ToString()];
        iconProbability = DataManager.instance.iconProbabilitys[currentStage.ToString()];

        mapWidth = stageLevel.mapWidth;
        mapHeight = stageLevel.mapHeight;

        // 자동 Add로 변경 나중에
        //probabilityMap.Add(ICON.SHOP, iconProbability.shopChance);
        //probabilityMap.Add(ICON.MONSTER, iconProbability.monsterChance);

        stageDataTempA.isSave = false;
        stageDataTempA.currentStage = currentStage;
        stageDataTempA.clearCount = 0;
        stageDataTempA.lineCount = stageLevel.lineCount;
        stageDataTempA.iconCounts.Clear();
        stageDataTempA.iconTypes.Clear();
    }



    /**********************************************************
    * 랜덤 아이콘을 고를 때 사용
    ***********************************************************/
    //private ICON PickIcon()
    //{
    //    int randomValue = Random.Range(0, 100);
    //    int sum = 0;
    //    foreach (var kvp in probabilityMap)
    //    {
    //        sum += kvp.Value;
    //        if (randomValue < sum)
    //        {
    //            return kvp.Key;
    //        }
    //    }
    //    throw new System.Exception($"{GetType()} - 랜덤 아이콘 못 받아옴");
    //}


    // 끝
}
