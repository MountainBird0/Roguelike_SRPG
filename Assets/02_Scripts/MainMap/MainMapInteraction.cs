/**********************************************************
* 클릭했을때 반응
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapInteraction : MonoBehaviour
{
    /**********************************************************
    * 클릭했을때 노드 상태 변경
    ***********************************************************/
    public void ChangeState(IconNode node)
    {
        node.iconState = IconState.VISITED;
        node.connectedNodes.ForEach(cn => cn.iconState = IconState.ATTAINABLE);

        GlobalSceneManager.instance.GoBattleScene();
    }

    /**********************************************************
    * 첫 게임 시작 시에만 가장 끝 visited 찾아서 자식들 attainable로 변경
    ***********************************************************/


}
