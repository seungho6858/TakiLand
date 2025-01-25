using System;
using System.Collections;
using System.Collections.Generic;
using Mib.Data;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BettingUI : MonoBehaviour
{
	[SerializeField] 
	private SerializableDictionary<Team, Button> _bettingButtons;
	
	[SerializeField] 
	private SerializableDictionary<BetPreset, Button> _betAmountButtons;

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
			Show();
		};

		StageManager.Instance.OnBattleStart += () =>
		{
			Hide();
		};
	}

	public void Show()
	{
		// TODO : 여기는 나중에 애니메이션 들어가면 수정
		gameObject.SetActive(true);
	}

	public void Hide()
	{
		// TODO : 여기는 나중에 애니메이션 들어가면 수정
		gameObject.SetActive(false);
	}
}
