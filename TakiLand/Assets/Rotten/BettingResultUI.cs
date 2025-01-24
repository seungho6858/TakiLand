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
		StageManager.Instance.OnBattleStart += () =>
		{
			BettingManager.Bet currentBet = BettingManager.Instance.CurrentBet;
			_betAmount.text = currentBet.BetAmount.ToString();
			_betTeam.text = currentBet.BetTeam.AsString();
		};
	}
}
