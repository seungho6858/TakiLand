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
		StageManager.Instance.OnStageChanged +=(red, blue, stage) =>
		{
			if (stage <= 1)
			{
				return;
			}

			Show();

			int prevStage = stage - 1;
			int scoreIndex = prevStage - 1;
			
			(bool won, int goldDelta) = BettingManager.Instance.CalculateResult(prevStage);

			_scores[scoreIndex].UpdateScore(won, goldDelta);
		};

		StageManager.Instance.OnBattleStart += () =>
		{
			Hide();
		};
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
