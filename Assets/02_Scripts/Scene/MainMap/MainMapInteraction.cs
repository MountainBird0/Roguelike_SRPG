/**********************************************************
* 클릭했을때 반응
***********************************************************/
using System.Collections.Generic;
using UnityEngine;

public class MainMapInteraction : MonoBehaviour
{
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
    * 누른게 몬스터 버튼일 때
    ***********************************************************/
    private void ClickMonster()
    {
        Debug.Log($"{GetType()} - 배틀씬으로 이동");
        GlobalSceneManager.instance.GoBattleScene();
    }

    /**********************************************************
    * 클릭했을때 노드 상태 변경
    ***********************************************************/
    private void ChangeState(IconNode node)
    {
        ChangeToLocked();

        node.iconState = IconState.VISITED;
        node.connectedNodes.ForEach(cn => cn.iconState = IconState.ATTAINABLE);
        DataManager.instance.OverWriteState();
    }

    /**********************************************************
    * 현재 ATTAINABLE노드들을 Locked로 변경
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
    * 누르면 어디로 갈지
    ***********************************************************/
    private void GoScenes(IconType icon)
    {

    }
}
