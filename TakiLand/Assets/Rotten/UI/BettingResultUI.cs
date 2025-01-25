using System;
using System.Collections;
using System.Collections.Generic;
using EnumsNET;
using Mib.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BettingResultUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _betTeam;
	
	[SerializeField]
	private TextMeshProUGUI _betAmount;
	
	[SerializeField]
	private TextMeshProUGUI _betDescription;
	
	[SerializeField]
	private Image _betTeamImage;

	private void Awake()
	{
		StageManager.Instance.OnStageChanged += (data, data1, arg3) =>
		{
			Hide();
		};
		
		StageManager.Instance.OnBattleStart += () =>
		{
			Show();
			BettingManager.Bet currentBet = BettingManager.Instance.CurrentBet;
			Color textColor = GeneralSetting.Instance.TeamColors[currentBet.BetTeam];
			
			// 베팅
			_betAmount.text = currentBet.BetAmount.ToString();
			_betTeam.text = currentBet.BetTeam.GetTeamName();
			_betTeam.color = textColor; 

			// sprite
			Sprite teamSprite = GeneralSetting.Instance.TeamSprites[currentBet.BetTeam].Selected;
			_betTeamImage.sprite = teamSprite;

			// 보상 설명
			Stage.Data stageData = new Stage.Key(StageManager.Instance.CurrentStage).Data;
			_betDescription.text = $"성공시 x {stageData.RewardRate.ToString()}배 획득";
			_betDescription.color = textColor;
		};
	}
	

	public void Show()
	{
		// TODO : 여기는 나중에 애니메이션 들어가면 수정
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		// TODO : 여기는 나중에 애니메이션 들어가면 수정
		gameObject.SetActive(false);
	}
}
