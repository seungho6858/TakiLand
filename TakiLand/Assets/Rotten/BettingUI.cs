using System;
using System.Collections;
using System.Collections.Generic;
using Mib.Data;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BettingUI : SerializedMonoBehaviour
{
	[SerializeField] 
	private Dictionary<Team, Button> _bettingButtons;
	
	[SerializeField] 
	private Dictionary<BetPreset, Button> _betAmountButtons;

	[SerializeField]
	private TextMeshProUGUI _currentBet;
	
	[SerializeField]
	private TextMeshProUGUI _currentRewardRate;

	private void Awake()
	{
		foreach (KeyValuePair<Team, Button> pair in _bettingButtons)
		{
			Team betTeam = pair.Key;
			pair.Value.onClick.AddListener(() =>
			{
				BettingManager.Instance.BetTeam(betTeam);
			});
		}

		foreach (KeyValuePair<BetPreset, Button> pair in _betAmountButtons)
		{
			var preset = pair.Key;
			pair.Value.onClick.AddListener(() =>
			{
				BettingManager.Instance.BetMoney(preset);
			});
		}

		BettingManager.Instance.OnBetChanged += (prev, current) =>
		{
			// TODO: 뭔가 애니메이션?
			_currentBet.text = current.ToString();
		};

		StageManager.Instance.OnStageChanged += (blue, red, stage) =>
		{
			var stageKey = new Stage.Key(stage);
			_currentRewardRate.text = $"x {stageKey.Data.RewardRate.ToString()}";
		};
	}
}
