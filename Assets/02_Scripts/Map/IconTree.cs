using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconNode
{
    public GameObject obj;
    public bool isReachable;

    public List<IconNode> children= new List<IconNode>();
}

//public class Tree
//{
//    public IconTree root; // 루트 노드

//    public Tree(IconData iconData)
//    {
//        root = new IconTree(iconData);
//    }

//    // 트리에 새로운 노드를 추가하는 메서드
//    public void AddNode(IconData parentValue, IconData newValue)
//    {
//        IconTree parentNode = FindNode(root, parentValue);
//        if (parentNode != null)
//        {
//            IconTree newNode = new IconTree(newValue);
//            parentNode.children.Add(newNode);
//        }
//        else
//        {
//            Debug.Log("Parent node not found!");
//        }
//    }

//    // 주어진 값과 일치하는 노드를 찾는 재귀적인 메서드
//    private IconTree FindNode(IconTree currentNode, IconData value)
//    {
//        Queue<IconTree> queue = new Queue<IconTree>();
//        queue.Enqueue(currentNode);

//        while (queue.Count > 0)
//        {
//            IconTree node = queue.Dequeue();

//            if (node.iconData == value)
//            {
//                return node;
//            }

//            foreach (IconTree child in node.children)
//            {
//                queue.Enqueue(child);
//            }
//        }

//        return null;
//    }
//}

//public class IconTree
//{
//    public IconData iconData;

//    public List<IconTree> children = new List<IconTree>();

//    public IconTree(IconData iconData)
//    {
//        this.iconData = iconData;
//        children = new List<IconTree>();
//    }
//}

