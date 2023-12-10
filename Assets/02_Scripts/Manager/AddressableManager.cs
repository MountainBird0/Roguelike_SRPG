using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableManager : MonoBehaviour
{
    public static AddressableManager instance;

    private AssetReferenceSprite spriteReference = null;

    private Dictionary<string, Sprite> gameSpriteDic = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning($"{GetType()} - Destory");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public async Task<Sprite> GetImage(string name)
    {
        if (gameSpriteDic.ContainsKey(name))
        {
            Debug.Log($"{GetType()} - dic에서 받아옴");
            return gameSpriteDic[name];
        }
        else
        {
            await LoadImage(name);

            if (gameSpriteDic.ContainsKey(name))
            {
                return gameSpriteDic[name];
            }
            else
            {
                Debug.LogError($"{GetType()} - 이미지 로드 실패: {name}");
                return null; // 기본, 에러 스프라이트를 반환
            }
        }
    }


    /******************************************************************************
    * 이미지들 Dic 만들기
    *******************************************************************************/
    public async void MakeImageAsync()
    {
        foreach (var kvp in DataManager.instance.currentUnitStats)
        {
            await LoadImage(kvp.Key);

            var usableSkills = DataManager.instance.currentUsableSkills[kvp.Key];
            foreach (var skill in usableSkills)
            {
                await LoadImage(skill.ToString());
            }
        }
    }

    private async Task LoadImage(string key)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(key);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            if (!gameSpriteDic.ContainsKey(key))
            {
                gameSpriteDic.Add(key, handle.Result);
                Debug.Log($"{GetType()} - 로드 성공");
            }
        }
        else
        {
            Debug.Log($"{GetType()} - 로드실패");
        }
    }



}

#region Sample
//// private AssetReferenceGameObject obj;
//// private List<GameObject> gameObjects = new List<GameObject>();

////private AssetReferenceGameObject[] objs;

//[SerializeField]
//private AssetReferenceSprite sprite;
//[SerializeField]
//private Image testImage;

////여기서 딕셔너리로 만들기

////private AssetReferenceT<AudioClip> soundBGM;
////private GameObject BGMObj;

//void Start()
//{
//    //StartCoroutine(InitAddressable());
//    SpawnObject();
//}

///**********************************************************
//* 초기화
//***********************************************************/
////IEnumerator InitAddressable()
////{
////    var init = Addressables.InitializeAsync();
////    yield return init;
////}

///**********************************************************
//* 불러옴
//***********************************************************/
//public void SpawnObject()
//{
//    // object
//    // obj.InstantiateAsync()
//    //obj.InstantiateAsync().Completed += (obj) =>
//    //{
//    //    gameObjects.Add(obj.Result);
//    //};

//    // Audio
//    //soundBGM.LoadAssetAsync().Completed += (clip) =>
//    //{
//    //    var bgmSound = BGMObj.GetComponent<AudioSource>();
//    //    bgmSound.clip = clip.Result;
//    //    bgmSound.loop = true;
//    //    bgmSound.Play();
//    //}

//    sprite.LoadAssetAsync().Completed += (img) =>
//    {
//        var image = testImage.GetComponent<Image>();
//        image.sprite = img.Result;
//    };

//}
///**********************************************************
//* 제거
//***********************************************************/
//public void Release()
//{
//    // 메모리 해제 후 제거
//    // Addressables.ReleaseInstance(gameObjects);
//    // soundBGM.ReleaseAsset();
//    sprite.ReleaseAsset();
//}
#endregion