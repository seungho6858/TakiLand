using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mib;
using Mib.Data;
using UnityEngine;

public class BettingManager : MonoSingleton<BettingManager>
{
	public class Bet
	{
		public Team BetTeam;
		public int BetAmount;
	}

	private Bet[] _betHistory;
	private int _currentGold;
	private int _currentBet;
	
	public event Action<int, int> OnBetChanged;
	
	protected override bool IsPersistent => false;

	public Bet CurrentBet => _betHistory.LastOrDefault();

	protected override void OnAwake()
	{
		_betHistory = new Bet[Define.Instance.GetValue("TotalStage")];
		for (int i = 0; i < _betHistory.Length; i++)
		{
			_betHistory[i] = new Bet();
		}
	}

	public void BetTeam(Team team)
	{
		CurrentBet.BetTeam = team;
	}

	public void BetMoney(BetPreset money)
	{
		CurrentBet.BetAmount = money switch
		{
			BetPreset.Reset => 0,
			_ => CurrentBet.BetAmount + (int)money
		};
	}
}
