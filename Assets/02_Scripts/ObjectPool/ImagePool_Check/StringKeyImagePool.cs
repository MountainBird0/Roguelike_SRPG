using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringKeyImagePool : MonoBehaviour
{
    public List<StringKeyImageData> imageList;

    public Dictionary<string, Sprite> images = new();


    public void MakeDictionary()
    {
        MakeDictionary(imageList, images);
    }

    private void MakeDictionary(List<StringKeyImageData> data, Dictionary<string, Sprite> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(data[i].name, data[i].image);
        }
    }



}
