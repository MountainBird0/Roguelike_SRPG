/**********************************************************
* Ŭ�������� ����
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
    * Ŭ���� ������ �޾ƿ���
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
    * ���� ������ ������ ��
    ***********************************************************/
    private void ClickMonster()
    {
        Debug.Log($"{GetType()} - ��Ʋ������ �̵�");
        GlobalSceneManager.instance.GoBattleScene();
    }


    /**********************************************************
    * �� ������ ������ ��
    ***********************************************************/
    private void ClickChest()
    {
        Debug.Log($"{GetType()} - ȸ�� ����");

        uiController.EnableHealWindow();

        foreach (var unitStats in DataManager.instance.currentUnitStats.ToList())
        {
            StatData updatedStat = unitStats.Value.HpFullUp();
            DataManager.instance.currentUnitStats[unitStats.Key] = updatedStat;
        }
    }


    /**********************************************************
    * ���� ������ ������ ��
    ***********************************************************/
    private void ClickShop()
    {
        uiController.EnableShopWindow();
    }



    /**********************************************************
    * Ŭ�������� ��� ���� ����
    ***********************************************************/
    private void ChangeState(IconNode node)
    {
        Debug.Log($"{GetType()} - ������ ����");

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
    * ���� ATTAINABLE������ Locked�� ����
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
    * ������ ���� ����
    ***********************************************************/
    private void GoScenes(IconType icon)
    {

    }
}
