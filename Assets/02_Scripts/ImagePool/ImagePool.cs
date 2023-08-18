using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePool : MonoBehaviour
{
    [SerializeField]
    public List<StringKeyImageData> unitSmallImage;
    public List<StringKeyImageData> unitBigImage;
    public List<IntKeyImageData> skillImage;

    public Dictionary<string, Sprite> smallImages = new();
    public Dictionary<string, Sprite> bigImages = new();
    public Dictionary<int, Sprite> skillImages = new();

    public void MakeDictionarys()
    {
        //MakeDictionary(unitSmallImage, smallImages);
        //MakeDictionary(unitBigImage, bigImages);
        //MakeDictionary(skillImage, skillImages);

    }
    //private void MakeDictionary(List<ImageData> data, Dictionary<string, Sprite> dic)
    //{
    //    for (int i = 0; i < data.Count; i++)
    //    {
    //        dic.Add(data[i].name, data[i].image);
    //    }
    //}

    private void MakeDictionary<T>(List<T> data, Dictionary<T, Sprite> dic) where T : ImageData
    {
        //for (int i = 0; i < data.Count; i++)
        //{
        //    dic.Add(data[i].name, data[i].image);
        //}
    }

}


