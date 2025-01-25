using System;
using System.Collections;
using System.Collections.Generic;
using Mib.Data;
using UnityEngine;

public class BettingHistoryUI : MonoBehaviour
{
	[SerializeField]
	private ScoreUI _scorePrefab;
	
	[SerializeField]
	private RectTransform _scoreRoot;

	private ScoreUI[] _scores;
	
	private void Awake()
	{
		_scores = new ScoreUI[Define.Instance.GetValue("TotalStage")];
		for (int i = 0; i < _scores.Length; i++)
		{
			_scores[i] = Instantiate(_scorePrefab, _scoreRoot);
			_scores[i].Initialize(i + 1);
		}
		
		StageManager.Instance.OnStageChanged +=(red, blue, stage) =>
		{
			int prevStage = stage - 1;
			int scoreIndex = prevStage - 1;
			
			BettingManager.Bet prevBet = BettingManager.Instance.GetBet(prevStage);
			Team result = StageManager.Instance.GetResult(prevStage);
			bool won = prevBet.BetTeam == result;

			int goldDelta = won
				? Stage.Instance.GetReward(prevStage, prevBet.BetAmount) - prevBet.BetAmount
				: -prevBet.BetAmount;
			
			_scores[scoreIndex].UpdateScore(won, goldDelta);

			if (stage > 1)
			{
				Show();
			}
		};
		
		Hide();
	}

	public void Show()
	{
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
	}
}
