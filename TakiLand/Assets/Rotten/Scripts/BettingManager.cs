using System.Collections;
using System.Collections.Generic;
using Mib;
using UnityEngine;

public class BettingManager : Singleton<BettingManager>
{
	public class Bet
	{
		public BetType BetType;
		public int BetAmount;
	}

	private readonly List<Bet> _betHistory = new();
	private readonly List<BetType> _results = new();
	private int _currentGold;
	private int _currentBet;
	
	protected override bool IsPersistent() => false;

	protected override void OnInitialized()
	{
		
	}

	public void DoBetting()
	{
		
	}
}
