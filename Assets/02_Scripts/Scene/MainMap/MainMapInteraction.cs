/**********************************************************
* 클릭했을때 반응
***********************************************************/
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MainMapInteraction : MonoBehaviour
{
    private int scaleDuration = 1;
    private Vector2 maxScale = new Vector3(1.3f, 1.3f);

    public MainMapMaker mapMaker;
    public MainMapUIController uiController;

    /**********************************************************
    * 클릭한 아이콘 받아오기
    ***********************************************************/
    public void GetIcon(GameObject icon)
    {
        IconNode node = DataManager.instance.nodes.Find(node => node.icon == icon);
        
        if(node.iconState != IconState.ATTAINABLE)
        {
            return;
        }
      
        ChangeState(node);
        IconType iconType = node.iconInfo.Item1;
        switch(iconType)
        {
            case IconType.MONSTER:
                ClickShop();
                break;
            case IconType.SHOP:
                ClickShop();
                break;
            case IconType.BOSS:

                break;
            case IconType.CHEST:
                ClickChest();
                break;
        }     
    }

    /**********************************************************
    * 몬스터 아이콘 눌렀을 때
    ***********************************************************/
    private void ClickMonster()
    {
        Debug.Log($"{GetType()} - 배틀씬으로 이동");
        GlobalSceneManager.instance.GoBattleScene();
    }


    /**********************************************************
    * 힐 아이콘 눌렀을 때
    ***********************************************************/
    private void ClickChest()
    {
        Debug.Log($"{GetType()} - 회복 누름");

        uiController.EnableHealWindow();

        foreach (var unitStats in DataManager.instance.currentUnitStats.ToList())
        {
            StatData updatedStat = unitStats.Value.HpFullUp();
            DataManager.instance.currentUnitStats[unitStats.Key] = updatedStat;
        }
    }


    /**********************************************************
    * 상점 아이콘 눌렀을 때
    ***********************************************************/
    private void ClickShop()
    {
        uiController.EnableShopWindow();
    }



    /**********************************************************
    * 클릭했을때 노드 상태 변경
    ***********************************************************/
    private void ChangeState(IconNode node)
    {
        Debug.Log($"{GetType()} - 노드상태 변경");

        ChangeToLocked();

        node.iconState = IconState.VISITED;
        node.connectedNodes.ForEach(cn =>
            {
                cn.iconState = IconState.ATTAINABLE;
                cn.icon.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            });
        mapMaker.ChangeLineColor();
        DataManager.instance.OverWriteState();
    }

    /**********************************************************
    * 현재 ATTAINABLE노드들을 Locked로 변경
    ***********************************************************/
    private void ChangeToLocked()
    {
        IconNode node = DataManager.instance.nodes[0];
        IconNode lastVisited = FindLastVisitedNode(node);

        lastVisited.connectedNodes.ForEach(cn => 
            {
                cn.iconState = IconState.LOCKED; 
                DOTween.Kill(cn.icon.transform);
                cn.icon.transform.localScale = Vector3.one;
            });
    }
    private IconNode FindLastVisitedNode(IconNode node)
    {
        Stack<IconNode> stack = new Stack<IconNode>();
        stack.Push(node);

        IconNode lastVisitedNode = null;

        while (stack.Count > 0)
        {
            IconNode currentNode = stack.Pop();

            if (currentNode.iconState == IconState.VISITED)
            {
                lastVisitedNode = currentNode;
            }

            foreach (IconNode connectedNode in currentNode.connectedNodes)
            {
                if(connectedNode.iconState == IconState.VISITED)
                {
                    stack.Push(connectedNode);
                }
            }
        }
        return lastVisitedNode;
    }

    /**********************************************************
    * 누르면 어디로 갈지
    ***********************************************************/
    private void GoScenes(IconType icon)
    {

    }
}
