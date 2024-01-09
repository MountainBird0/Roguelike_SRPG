/******************************************************************************
* ResultSlot에 들어갈 정보
*******************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultSlot : MonoBehaviour
{
    public Image image;
    public Image yellowBar;

    [Header("Stat")]
    public TextMeshProUGUI className;
    public TextMeshProUGUI level;
    public TextMeshProUGUI curExp;
    public TextMeshProUGUI maxExp;
}
