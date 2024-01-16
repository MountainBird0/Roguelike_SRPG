using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableSkillWindow : MonoBehaviour
{
    public int skillNum = 0;

    [Header("Stat")]
    public Image skillImage;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI coolTime;
    public TextMeshProUGUI type;
    public TextMeshProUGUI range;
    public TextMeshProUGUI target;
    public TextMeshProUGUI explain;
}
