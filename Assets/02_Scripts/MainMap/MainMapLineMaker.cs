using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMapLineMaker : MonoBehaviour
{
    public GameObject line;

    public void DrawLine(List<IconNode> nodes)
    {
        Debug.Log($"{GetType()} - 선 그림");
        for (int i = 1; i < nodes.Count; i++)
        {
            Debug.Log($"{GetType()} - 선 그림");

            for (int j = 0; j < nodes[i].connectedNodes.Count; j++)
            {
                Debug.Log($"{GetType()} - 선 그림");

                var lineObject = Instantiate(line);
                var lineRenderer = lineObject.GetComponent<LineRenderer>();

                var fromPoint = nodes[i].icon.transform.position;
                var toPoint = nodes[i].connectedNodes[j].icon.transform.position;
                lineRenderer.SetPosition(0, fromPoint);
                lineRenderer.SetPosition(1, toPoint);

            }
        }
        


    }
}
