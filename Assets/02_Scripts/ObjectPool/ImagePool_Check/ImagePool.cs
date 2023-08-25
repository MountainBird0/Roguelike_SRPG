using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// À¯´Ö imagePoolÀÌ¶û skill iamgePool ºÐ¸®
public class ImagePool : MonoBehaviour
{
    public List<StringKeyImageData> unitSmallImage;
    public List<StringKeyImageData> unitBigImage;
    public List<IntKeyImageData> skillImage;

    public Dictionary<string, Sprite> smallImages = new();
    public Dictionary<string, Sprite> bigImages = new();
    public Dictionary<int, Sprite> skillImages = new();

    public void MakeDictionarys()
    {
        MakeStringDictionary(unitSmallImage, smallImages);
        MakeStringDictionary(unitBigImage, bigImages);
        MakeIntDictionary(skillImage, skillImages);

    }
    private void MakeStringDictionary(List<StringKeyImageData> data, Dictionary<string, Sprite> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(data[i].name, data[i].image);
        }
    }
    private void MakeIntDictionary(List<IntKeyImageData> data, Dictionary<int, Sprite> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(data[i].id, data[i].image);
        }
    }

}


