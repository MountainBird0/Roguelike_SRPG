/**********************************************************
* ���� �� ���� �����͸� ������
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
        // �����ϱ� �ǹ�
        if (!stageDataTempA.isSave)
        {
            Debug.Log($"{GetType()} - ���ο� �� ������ ����");
            //MakeMapDataTempA();
        }
    }

    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    ***********************************************************/
    private void MakeMapData()
    {
        SetData();

        float mapWidth;
        float mapHeight;

        float widthGap;

        Vector2 iconPos = Vector2.zero;

        // �� ����
        for (int i = 0; i < stageLevels.Count; i++)
        {
            AddProbabilityDic(i + 1);

            mapdatas[i].lineCount = stageLevels[i].lineCount;

            mapWidth = stageLevels[i].mapWidth;
            mapHeight = stageLevels[i].mapHeight;
           
            iconPos.x = -(mapWidth / 2); // �������� �ʱ� x��ǥ �� (���� �Ʒ�)
            widthGap = mapWidth / (mapdatas[i].lineCount - 1);
            
            // �� ������ �� ����
            for (int j = 0; j < stageLevels[i].lineCount; j++)
            {
                // ù��° ����
                if(j == 0)
                {
                    SetIcon(i, mapHeight, stageLevels[i].firstLine, ICON.MONSTER, ref iconPos);
                }
                // ������ ����
                else if(j == (stageLevels[i].lineCount -1))
                {
                    SetIcon(i, mapHeight, stageLevels[i].lastLine, ICON.BOSS, ref iconPos);
                }
                else
                {
                    // ���� ����
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
    * ���� ������ ������ �ֱ�
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
    * ���� ������ ������ �ֱ�
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
    * ���� ������ dic����
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
    * ���� ������ dic �߰�
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
    * ���� �������� ��ü Ȯ�� ���ϱ�
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
    * �� ������ ���� default������ �غ�
    ***********************************************************/
    private void SetData()
    {
        // �õ� ����
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
    * ������ �����ܿ� ��ǥ���� ��� �θ� ��忡 ���� ������
    ***********************************************************/
    private void SetIconGrid()
    {

    }




    /**********************************************************
    * ���ο� �� ������ ���� ������ ����
    * - ������ ó�� ������ ��, �� stage�� Ŭ���� ���� �� ���
    * - ���� ����Ȯ�� �߰� �ʿ�
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
            // ù��° ����
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
            // ������ ����
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
                // ���� ����
                if (i == (stageLevel.chestLine - 1))
                {
                    for (int j = 0; j < iconCount; j++)
                    {
                        stageDataTempA.iconPos.Enqueue(iconPos);
                        iconPos.y += heightGap;
                        stageDataTempA.iconTypes.Enqueue(ICON.CHEST);
                    }
                }
                // �Ϲ� ����
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
    * icon���� ������ ���� ��� ����
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

            for (int j = 0; j < iconCount; j++) // �� ���� ���� ������
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

        //��� ����?
        //if (j == 0)
        //{
        //    // ���� j = 0�� ����
        //}
        //else if (j == stageLevel.firstLine - 1)
        //{
        //    // ���� j = stageLevel.firstLine - 1�� ����
        //}
        //else
        //{

        //}

    }

    /**********************************************************
    * �� ������ ���� ������ ����
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

        // �ڵ� Add�� ���� ���߿�
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
    * ���� �������� �� �� ���
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
    //    throw new System.Exception($"{GetType()} - ���� ������ �� �޾ƿ�");
    //}


    // ��
}
