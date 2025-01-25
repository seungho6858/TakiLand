using System;
using System.Collections;
using System.Collections.Generic;
using EnumsNET;
using TMPro;
using UnityEngine;

public class BettingResultUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _betTeam;
	
	[SerializeField]
	private TextMeshProUGUI _betAmount;

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
			_betAmount.text = currentBet.BetAmount.ToString();
			_betTeam.text = currentBet.BetTeam.AsString();
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
