using System;
using System.Collections;
using System.Collections.Generic;
using Mib.Data;
using UnityEngine;

public class BettingHistoryUI : MonoBehaviour
{
	[SerializeField]
	private ScoreUI[] _scores;
	
	private void Awake()
	{
		Hide();
		
		StageManager.Instance.OnStageChanged +=(red, blue, stage) =>
		{
			if (stage <= 1)
			{
				return;
			}

			Show();

			for (int i = 0; i < _scores.Length; ++i)
			{
				int prevStage = stage - 1;
				int targetStage = i + 1;
				bool needToPush = _scores.Length < prevStage;
				if (needToPush)
				{
					int offset = prevStage - _scores.Length;
					targetStage += offset;
					(bool won, int goldDelta) = BettingManager.Instance.CalculateResult(targetStage);
					_scores[i].UpdateScore(targetStage, won, goldDelta);
				}
				else
				{
					bool hasData = i < prevStage;
					if (hasData)
					{
						(bool won, int goldDelta) = BettingManager.Instance.CalculateResult(targetStage);
						_scores[i].UpdateScore(targetStage, won, goldDelta);
					}
					else
					{
						_scores[i].Initialize(targetStage);
					}
				}
			}
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
