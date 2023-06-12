using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapLineMaker : MonoBehaviour
{
    public GameObject line;

    public void DrawLine(List<IconNode> nodes)
    {
        for (int i = 1; i < nodes.Count; i++)
        {
            for (int j = 0; j < nodes[i].connectedNodes.Count; j++)
            {
                var lineObject = Instantiate(line);
                var lineRenderer = lineObject.GetComponent<LineRenderer>();

                var fromPoint = nodes[i].icon.transform.position;
                var toPoint = nodes[i].connectedNodes[j].icon.transform.position;
                lineRenderer.SetPosition(0, fromPoint);
                lineRenderer.SetPosition(1, toPoint);

                if (nodes[i].iconState == IconState.VISITED)
                {
                    if(nodes[i].connectedNodes[j].iconState == IconState.VISITED)
                    {
                        Debug.Log($"{GetType()} - 그림?");
                        lineRenderer.material.color = Color.black;
                    }
                    else if(nodes[i].connectedNodes[j].iconState == IconState.ATTAINABLE)
                    {
                        Debug.Log($"{GetType()} - 그림?");
                        lineRenderer.material.color = Color.blue;
                    }
                }
            }
        }
    }


}
