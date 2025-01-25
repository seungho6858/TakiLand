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
	}
}
