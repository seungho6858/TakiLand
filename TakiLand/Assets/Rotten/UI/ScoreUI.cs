using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _resultText;
    
    [SerializeField]
    private TextMeshProUGUI _goldText;
    
    private int _stage;

    public void Initialize(int stage)
    {
        _stage = stage;
        _resultText.text = string.Empty;
        _goldText.text = string.Empty;
    }

    public void UpdateScore(bool isWin, int goldDelta)
    {
        _resultText.text = isWin ? "O" : "X";
        _resultText.color = isWin 
            ? GeneralSetting.Instance.WinColor
            : GeneralSetting.Instance.LoseColor;
        
        _goldText.text = $"{goldDelta:+#;-#;0}";
    }
}
