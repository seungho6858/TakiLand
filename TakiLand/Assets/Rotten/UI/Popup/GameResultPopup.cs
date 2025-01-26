using System.Collections;
using System.Collections.Generic;
using Mib;
using Mib.Data;
using Mib.UI;
using TMPro;
using UnityEngine;

[PopupPath("GameResultPopup")]
public class GameResultPopup : PopupBase
{
    [SerializeField]
    private SerializableDictionary<bool, GameObject> _resultTextGo;
    
    [SerializeField]
    private SerializableDictionary<bool, TextMeshProUGUI> _resultTexts;
    
    [SerializeField]
    private TMP_InputField _nickNameInputField;

    private const string DefaultPlayerNickName = "Slime_{0}";

    private static int CurrentPlayerNumber => Random.Range(0, 111111271);
    
    public void OnClick()
    {
        string nick = _nickNameInputField.text;

        if (!string.IsNullOrEmpty(nick))
        {
            BattleManager.nick = nick;
            
            ServerManager.instance.LeaderboardPlayerRecord(BettingManager.Instance.CurrentGold, 
                JsonUtility.ToJson(new BattleManager.Rank()
                {
                    nick = nick, stage = StageManager.Instance.CurrentStage
                }), nick, 
                b =>
                {
                    Mib.UI.PopupManager.Instance.Open<Popup_Ranking>();
                });
        }
    }
    
    protected override void OnAwake()
    {
        
    }

    private string GetPlayerNickName()
    {
        return string.Format(DefaultPlayerNickName, CurrentPlayerNumber.ToString());
    }

    public override void OnOpen()
    {
        _nickNameInputField.text = GetPlayerNickName();
        
        bool hasClear = StageManager.Instance.CurrentStage >= Define.Instance.GetValue("TotalStage");

        foreach (KeyValuePair<bool, GameObject> pair in _resultTextGo)
        {
            pair.Value.SetActive(pair.Key == hasClear);
        }
        
        _resultTexts[hasClear].text = hasClear 
            ? $"{BettingManager.Instance.CurrentGold}"
            : $"최종 라운드 : {StageManager.Instance.CurrentStage}";
    }

    protected override void OnClose()
    {
    }


}
