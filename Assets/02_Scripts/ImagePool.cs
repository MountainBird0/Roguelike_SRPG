using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImageData
{
    public string name;
    public Sprite image;
}

public class ImagePool : MonoBehaviour
{
    public List<ImageData> unitSmallImage;
    public List<ImageData> unitBigImage;
    public List<ImageData> SkillImage;

    public Dictionary<string, Sprite> smallImages = new();
    public Dictionary<string, Sprite> bigImages = new();
    public Dictionary<string, Sprite> SkillImages = new();

    public void MakeDictionarys()
    {
        MakeDictionary(unitSmallImage, smallImages);
        MakeDictionary(unitBigImage, bigImages);
        MakeDictionary(SkillImage, SkillImages);

    }
    private void MakeDictionary(List<ImageData> data, Dictionary<string, Sprite> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(data[i].name, data[i].image);
        }
    }

}


