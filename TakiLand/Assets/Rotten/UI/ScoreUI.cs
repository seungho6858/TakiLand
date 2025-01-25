using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _stageText;
    
    [SerializeField]
    private Image _resultImage;
    
    [SerializeField]
    private TextMeshProUGUI _goldText;
    
    public void Initialize(int stage)
    {
        _stageText.text = stage.ToString();
        _resultImage.gameObject.SetActive(false);
        _goldText.text = string.Empty;
    }

    public void UpdateScore(int stage, bool isWin, int goldDelta)
    {
        _resultImage.gameObject.SetActive(true);
        _stageText.text = stage.ToString();
        _resultImage.sprite = GeneralSetting.Instance.ScoreSprite[isWin];
        _goldText.text = $"{goldDelta:+#;-#;0}";
    }
}
