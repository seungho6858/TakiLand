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
    private TextMeshProUGUI _resultText; // 배팅에 성공 / 실패
    
    [SerializeField]
    private TextMeshProUGUI _goldText; // 골드변화량
    
    [SerializeField]
    private TextMeshProUGUI _buttonText; // 획득하기 vs 확인
    
    [SerializeField]
    private TextMeshProUGUI _titleText; // 레드팀 승리

    [SerializeField]
    private Image _titleImage; // 얘는 컬러변경
    
    [SerializeField]
    private Image _slimeImage; // 얘는 컬러변경
    
    protected override void OnAwake()
    {
        _confirmButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound("Menu_Select_00");
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
        string resultText = won ? "성공" : "실패";
        Team result = StageManager.Instance.GetResult(stage);

        _titleImage.color = GeneralSetting.Instance.StageResultColors[result];
        _titleText.text = result == Team.Draw
            ? $"{result.GetTeamName()} !!"
            : $"{result.GetTeamName()} 승리";
        
        _resultText.text =  $"배팅에 {resultText}하였습니다.";
        _goldText.text = goldDelta.ToString("N0");

        var buttonImage = _confirmButton.GetComponent<Image>();
        buttonImage.sprite = GeneralSetting.Instance.ButtonImage[result];
        _buttonText.text = won ? "획득하기" : "확인";

        Team myBetTeam = BettingManager.Instance.CurrentBet.BetTeam;
        GeneralSetting.StageResultImage resultImage = GeneralSetting.Instance.ResultSprites[myBetTeam];
        
        _slimeImage.sprite = won? resultImage.Win : resultImage.Lose;
    }
}
