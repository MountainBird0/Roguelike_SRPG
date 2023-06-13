/**********************************************************
* Ŭ�������� ����
***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMapInteraction : MonoBehaviour
{
    private float scaleDuration = 1f;
    private Vector3 maxScale = new Vector3(2f, 2f, 2f);

    /**********************************************************
    * Ŭ�������� ��� ���� ����
    ***********************************************************/
    public void ChangeState(IconNode node)
    {
        node.iconState = IconState.VISITED;
        node.connectedNodes.ForEach(cn =>
        {
            cn.iconState = IconState.ATTAINABLE;
        });

        GlobalSceneManager.instance.GoBattleScene();
    }

    /**********************************************************
    * ù ���� ���� �ÿ��� ���� �� visited ã�Ƽ� �ڽĵ� attainable�� ����
    ***********************************************************/


}
