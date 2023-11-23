using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AddressableAssets;
using System.Linq;

public class DownloadManager : MonoBehaviour
{
    public Image redBar;

    public TextMeshProUGUI datasize;

    public TitleUIController uiController;

    [Header("Label")]
    public AssetLabelReference skillIconLabel;
    public AssetLabelReference unitIconLabel;

    private long patchSize; // 받을 파일의 Size
    private Dictionary<string, long> patchMap = new();

    public static DownloadManager instance;
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
    }

    /**********************************************************
    * 어드레서블 초기화
    ***********************************************************/
    public IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    /**********************************************************
    * 업데이트 파일 확인
    ***********************************************************/
    public IEnumerator CheckUpdateFiles()
    {
        var labels = new List<string>() { skillIconLabel.labelString, unitIconLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            Debug.Log($"{GetType()} - 파일체크");
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle; // 작업 완료까지 대기

            patchSize += handle.Result;
        }

        if(patchSize > decimal.Zero)
        {
            Debug.Log($"{GetType()} - 업데이트 파일 존재");

            uiController.HideCheckPopUp();
            uiController.ShowDownloadPopUp();
            datasize.text = GetFileSize(patchSize);
        }
        else
        {
            Debug.Log($"{GetType()} - 업데이트 파일 없음");

            uiController.HideCheckPopUp();
            yield return new WaitForSeconds(2f);
        }
    }

    private string GetFileSize(long byteCnt)
    {
        string size = "0 bytes";

        if(byteCnt >= 1073741824.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1073741824.0) + " GB";
        }
        else if (byteCnt >= 1048576.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1048576.0) + " MB";
        }
        else if (byteCnt >= 1024.0)
        {
            size = string.Format("{0:##.##}", byteCnt / 1024.0) + " KB";
        }
        else if( 0 < byteCnt && byteCnt < 1024.0)
        {
            size = byteCnt.ToString() + " Bytes";
        }

        return size;
    }

    public void Download()
    {
        StartCoroutine(PatchFiles());
    }

    IEnumerator PatchFiles()
    {
        Debug.Log($"{GetType()} - 다운시작");
        var labels = new List<string>() { skillIconLabel.labelString, unitIconLabel.labelString };

        foreach (var label in labels)
        {
            Debug.Log($"{GetType()} - 다운할거 {label}");
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle; // 작업 완료까지 대기

            // 라벨에 있는것을 다운
            if(handle.Result != decimal.Zero)
            {
                StartCoroutine(DownloadLabel(label));
            }
        }

        yield return CheckDownload();
    }

    IEnumerator DownloadLabel(string label)
    {
        patchMap.Add(label, 0); // 각레이블에 대한 다운로드 상태저장

        var handle = Addressables.DownloadDependenciesAsync(label, false); // 파일 다운로드

        while(!handle.IsDone) // 다운 완료 될 때까지 반복
        {
            Debug.Log($"{GetType()} - 궁금");
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame(); // 다운로드 완료될때까지 너무 많은 연산자원을 쓰는것을 방지
        }

        // 다운로드 완료 시
        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle); // 다운로드작업의 핸들 해제
    }

    IEnumerator CheckDownload()
    {
        var total = 0f;
        datasize.text = "0";

        while(true)
        {
            total += patchMap.Sum(tmp => tmp.Value);
            redBar.fillAmount = total / patchSize;

            Debug.Log($"{GetType()} - 다운 중");

            if (total == patchSize)
            {
                Debug.Log($"{GetType()} - 다운 끝");
                uiController.HideDownloadPopUp();
                // 다운 완료시 종료
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
            // 다음 프레임에서 다운로드 상태를 체크해주기위해
        }

    }
}
