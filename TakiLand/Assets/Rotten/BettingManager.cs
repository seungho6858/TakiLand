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
		public int ExtraRewardRate;
	}

	public class Gold
	{
		public int Value
		{
			get;
			private set;
		}

		public void SetValue(int gold)
		{
			int prevGold = Value;
			Value = gold;
			Instance.OnGoldChanged?.Invoke(prevGold, Value);
		}
	}

	private Bet[] _betHistory;
	private readonly Gold _gold = new Gold();
	private int CurrentGold => _gold.Value;
	
	public event Action<int, int> OnBetChanged;//prev, current
	public event Action<Team> OnBetTeamChanged; //current
	public event Action<int, int> OnGoldChanged;//prev, current
	
	protected override bool IsPersistent => false;

	public Bet CurrentBet => GetBet(StageManager.Instance.CurrentStage);

	public Bet GetBet(int stage) => _betHistory[stage - 1];
	
	protected override void OnAwake()
	{
		_betHistory = new Bet[Define.Instance.GetValue("TotalStage")];
		for (int i = 0; i < _betHistory.Length; i++)
		{
			_betHistory[i] = new Bet();
		}

		StageManager.Instance.OnBattleStart += () =>
		{
			_gold.SetValue(CurrentGold - CurrentBet.BetAmount);
		};

		StageManager.Instance.OnStageChanged += (_, _, stage) =>
		{
			var stageData = new Stage.Key(stage);
			BetMoney(stageData.Data.MinimumCost);
		};
	}

	public void Initialize()
	{
		_gold.SetValue(Define.Instance.GetValue("DefaultGold"));
	}

	public void BetTeam(Team team)
	{
		CurrentBet.BetTeam = team;
		OnBetTeamChanged?.Invoke(team);
	}

	public void BetMoney(BetPreset money)
	{
		int amount = money switch
		{
			BetPreset.Reset => 0,
			_ => CurrentBet.BetAmount + (int)money
		};
		
		BetMoney(amount);
	}
	
	public void BetMoney(int amount)
	{
		int prevBet = CurrentBet.BetAmount;

		Stage.Data data = Stage.Instance.Table[new Stage.Key(StageManager.Instance.CurrentStage)];

		if (data.MinimumCost <= amount && amount <= data.MaximumCost)
		{
			amount = Math.Min(amount, CurrentGold);
		}
		else if (amount < data.MinimumCost)
		{
			amount = Math.Min(data.MinimumCost, CurrentGold);
		}
		else if (amount > data.MaximumCost)
		{
			amount = Math.Min(data.MaximumCost, CurrentGold);
		}
		
		CurrentBet.BetAmount = amount;
		
		OnBetChanged?.Invoke(prevBet, CurrentBet.BetAmount);
	}

	public void SettleBets(Team team, int currentStage)
	{
		bool isWin = CurrentBet.BetTeam == team;

		if (isWin)
		{
			// 남은 탐욕슬라임 개수만큼 추가 리워드 적용
			int extraRewardRate = BattleManager.GetGreedCount(Team.Blue);
			CurrentBet.ExtraRewardRate = extraRewardRate;
			
			// 리워드 적용 
			var reward = Stage.Instance.GetReward(currentStage, CurrentBet);
			_gold.SetValue(CurrentGold + reward);
		}
	}

	public (bool won, int goldDelta) CalculateResult(int stage)
	{
		Bet prevBet = GetBet(stage);
		Team result = StageManager.Instance.GetResult(stage);
		bool won = prevBet.BetTeam == result;

		int goldDelta = won
			? Stage.Instance.GetReward(stage, prevBet) - prevBet.BetAmount
			: -prevBet.BetAmount;
		
		return (won, goldDelta);
	}

	public void Cheat_SetGold(int amount)
	{
		_gold.SetValue(amount);
	}
}
