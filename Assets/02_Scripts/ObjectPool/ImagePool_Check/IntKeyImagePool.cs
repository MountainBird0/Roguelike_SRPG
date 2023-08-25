using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntKeyImagePool : MonoBehaviour
{
    public List<IntKeyImageData> imageList;

    public Dictionary<int, Sprite> images = new();


    public void MakeDictionary()
    {
        MakeDictionary(imageList, images);
    }

    private void MakeDictionary(List<IntKeyImageData> data, Dictionary<int, Sprite> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(data[i].id, data[i].image);
        }
    }
}
