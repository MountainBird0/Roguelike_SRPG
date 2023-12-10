/**********************************************************
* Ŭ�������� ����
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MainMapInteraction : MonoBehaviour
{
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
                ClickMonster();
                break;
            case IconType.SHOP:

                break;
            case IconType.BOSS:

                break;
            case IconType.CHEST:

                break;
        }     
    }

    /**********************************************************
    * ������ ���� ��ư�� ��
    ***********************************************************/
    private void ClickMonster()
    {
        Debug.Log($"{GetType()} - ��Ʋ������ �̵�");
        GlobalSceneManager.instance.GoBattleScene();
    }

    /**********************************************************
    * Ŭ�������� ��� ���� ����
    ***********************************************************/
    private void ChangeState(IconNode node)
    {
        ChangeToLocked();

        node.iconState = IconState.VISITED;
        node.connectedNodes.ForEach(cn => cn.iconState = IconState.ATTAINABLE);
        DataManager.instance.OverWriteState();
    }

    /**********************************************************
    * ���� ATTAINABLE������ Locked�� ����
    ***********************************************************/
    private void ChangeToLocked()
    {
        IconNode node = DataManager.instance.nodes[0];
        IconNode lastVisited = FindLastVisitedNode(node);

        lastVisited.connectedNodes.ForEach(cn => cn.iconState = IconState.LOCKED);
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
