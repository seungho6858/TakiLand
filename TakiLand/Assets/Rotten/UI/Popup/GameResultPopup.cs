using System.Collections;
using System.Collections.Generic;
using Mib;
using Mib.Data;
using Mib.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[PopupPath("GameResultPopup")]
public class GameResultPopup : PopupBase
{
    [SerializeField]
    private Button _titleButton;
    
    [SerializeField]
    private TextMeshProUGUI _resultText;
    
    [SerializeField]
    private TMP_InputField _nickNameInputField;

    private const string DefaultPlayerNickName = "Slime_{0}";
    
    private int CurrentPlayerNumber
    {
        get => PlayerPrefs.GetInt("PlayerNumber_Key", 0);
        set => PlayerPrefs.SetInt("PlayerNumber_Key", value);
    }
    
    public void OnClick()
    {
        string nick = _nickNameInputField.text;

        if (!string.IsNullOrEmpty(nick))
        {
            BattleManager.nick = nick;
            
            ServerManager.instance.LeaderboardPlayerRecord(BettingManager.Instance.CurrentGold, 
                JsonUtility.ToJson(new BattleManager.Rank()), nick, 
                b =>
                {
                    Mib.UI.PopupManager.Instance.Open<Popup_Ranking>();
                });
        }
    }
    
    protected override void OnAwake()
    {
        _titleButton.onClick.AddListener(() =>
        {
            BattleManager.LeaderboardPlayerRecord(
                BettingManager.Instance.CurrentGold,
                StageManager.Instance.CurrentStage, 
                GetPlayerNickName());
            
            SceneLoader.ChangeScene(Constant.TitleScene).Forget();
        });
    }

    private string GetPlayerNickName()
    {
        int currentPlayerNumber = CurrentPlayerNumber++;

        return string.Format(DefaultPlayerNickName, currentPlayerNumber.ToString());
    }

    public override void OnOpen()
    {
        _nickNameInputField.text = GetPlayerNickName();
        
        bool hasClear = StageManager.Instance.CurrentStage >= Define.Instance.GetValue("TotalStage");

        _resultText.text = hasClear
            ? $"최종 금화 : {BettingManager.Instance.CurrentGold}"
            : $"최고 라운드 : {StageManager.Instance.CurrentStage}";
    }

    protected override void OnClose()
    {
    }


}
