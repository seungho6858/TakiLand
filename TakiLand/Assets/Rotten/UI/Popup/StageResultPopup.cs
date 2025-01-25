using System.Collections;
using System.Collections.Generic;
using Mib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[PopupPath("StageResultPopup")]
public class StageResultPopup : PopupBase
{
    [SerializeField]
    private Button _confirmButton;
    
    [SerializeField]
    private TextMeshProUGUI _resultText;
    
    protected override void OnAwake()
    {
        _confirmButton.onClick.AddListener(() =>
        {
            PopupManager.Instance.CloseCurrent();
        });
    }

    public override void OnOpen()
    {
    }

    protected override void OnClose()
    {
    }

    public void Set(int stage)
    {
        (bool won, int goldDelta) = BettingManager.Instance.CalculateResult(stage);
        string resultText = won ? "맞췄다!" : "틀렸다!";
        Team result = StageManager.Instance.GetResult(stage);
        _resultText.text =  $"승리팀:{result.GetTeamName()} {resultText} gold: {goldDelta} ";

    }
}
