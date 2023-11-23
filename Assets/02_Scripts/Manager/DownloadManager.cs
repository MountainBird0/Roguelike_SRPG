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

    private long patchSize; // ���� ������ Size
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
    * ��巹���� �ʱ�ȭ
    ***********************************************************/
    public IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    /**********************************************************
    * ������Ʈ ���� Ȯ��
    ***********************************************************/
    public IEnumerator CheckUpdateFiles()
    {
        var labels = new List<string>() { skillIconLabel.labelString, unitIconLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            Debug.Log($"{GetType()} - ����üũ");
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle; // �۾� �Ϸ���� ���

            patchSize += handle.Result;
        }

        if(patchSize > decimal.Zero)
        {
            Debug.Log($"{GetType()} - ������Ʈ ���� ����");

            uiController.HideCheckPopUp();
            uiController.ShowDownloadPopUp();
            datasize.text = GetFileSize(patchSize);
        }
        else
        {
            Debug.Log($"{GetType()} - ������Ʈ ���� ����");

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
        Debug.Log($"{GetType()} - �ٿ����");
        var labels = new List<string>() { skillIconLabel.labelString, unitIconLabel.labelString };

        foreach (var label in labels)
        {
            Debug.Log($"{GetType()} - �ٿ��Ұ� {label}");
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle; // �۾� �Ϸ���� ���

            // �󺧿� �ִ°��� �ٿ�
            if(handle.Result != decimal.Zero)
            {
                StartCoroutine(DownloadLabel(label));
            }
        }

        yield return CheckDownload();
    }

    IEnumerator DownloadLabel(string label)
    {
        patchMap.Add(label, 0); // �����̺� ���� �ٿ�ε� ��������

        var handle = Addressables.DownloadDependenciesAsync(label, false); // ���� �ٿ�ε�

        while(!handle.IsDone) // �ٿ� �Ϸ� �� ������ �ݺ�
        {
            Debug.Log($"{GetType()} - �ñ�");
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame(); // �ٿ�ε� �Ϸ�ɶ����� �ʹ� ���� �����ڿ��� ���°��� ����
        }

        // �ٿ�ε� �Ϸ� ��
        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle); // �ٿ�ε��۾��� �ڵ� ����
    }

    IEnumerator CheckDownload()
    {
        var total = 0f;
        datasize.text = "0";

        while(true)
        {
            total += patchMap.Sum(tmp => tmp.Value);
            redBar.fillAmount = total / patchSize;

            Debug.Log($"{GetType()} - �ٿ� ��");

            if (total == patchSize)
            {
                Debug.Log($"{GetType()} - �ٿ� ��");
                uiController.HideDownloadPopUp();
                // �ٿ� �Ϸ�� ����
                break;
            }

            total = 0f;
            yield return new WaitForEndOfFrame();
            // ���� �����ӿ��� �ٿ�ε� ���¸� üũ���ֱ�����
        }

    }
}
