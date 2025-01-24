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
	
	public event Action<int, int> OnBetChanged;
	
	protected override bool IsPersistent => false;

	public Bet CurrentBet => _betHistory[StageManager.Instance.CurrentStage];

	protected override void OnAwake()
	{
		_betHistory = new Bet[Define.Instance.GetValue("TotalStage")];
		for (int i = 0; i < _betHistory.Length; i++)
		{
			_betHistory[i] = new Bet();
		}

		StageManager.Instance.OnBattleStart += () =>
		{
			// TODO: 뭔가 애니메이션 여기서.
			_currentGold -= CurrentBet.BetAmount;
		};
	}

	public void Initialize()
	{
		_currentGold = Define.Instance.GetValue("DefaultGold");
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

	public void SettleBets(Team team, int currentStage)
	{
		bool isWin = CurrentBet.BetTeam == team;
		int rewardRate = new Stage.Key(currentStage).Data.RewardRate;

		if (isWin)
		{
			// TODO : 뭔가 애니메이션 여기서도.
			_currentGold += CurrentBet.BetAmount * rewardRate;
		} 
	}
}
