/******************************************************************************
* TargetingWindow에 들어갈 정보
*******************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetingWindow : MonoBehaviour
{
    public Image unitImage;
    public Image skillImage;
    public Image redBar;

    [Header("Stat")]
    public TextMeshProUGUI className;
    public TextMeshProUGUI level;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defensive;
}
