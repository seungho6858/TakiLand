using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfomationUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _currentStageText;
	
	[SerializeField]
	private TextMeshProUGUI _remainTimeText;

	private void Awake()
	{
		_currentStageText.text = "";
		_remainTimeText.text = "";
		
		BattleManager.onBattleTimer += remainTime =>
		{
			_remainTimeText.text = remainTime.ToString("N0");
		};

		StageManager.Instance.OnBattleStart += () =>
		{
			_currentStageText.text = $"Round {StageManager.Instance.CurrentStage.ToString()}";
		};
	}
}
