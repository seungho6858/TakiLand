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
	private SerializableDictionary<Team, Image> _bettingButtonImages;
	
	[SerializeField] 
	private SerializableDictionary<BetPreset, Button> _betAmountButtons;
	
	[SerializeField]
	private TextMeshProUGUI _betRange;

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
		
		BettingManager.Instance.OnBetTeamChanged += (current) =>
		{
			SelectBetTeam(current);
		};

		StageManager.Instance.OnStageChanged += (blue, red, stage) =>
		{
			Stage.Data stageData = new Stage.Key(stage).Data;
			_currentRewardRate.text = $"x {stageData.RewardRate.ToString()}";
			_betRange.text = $"최소 {stageData.MinimumCost.ToString()} ~ 최대 {stageData.MaximumCost.ToString()}";
			
			SelectBetTeam(null);
			Show();
		};

		StageManager.Instance.OnBattleStart += () =>
		{
			Hide();
		};
	}

	private void SelectBetTeam(Team? team)
	{
		foreach (var pair in GeneralSetting.Instance.TeamSprites)
		{
			_bettingButtonImages[pair.Key].sprite = team.HasValue && team.Value == pair.Key
				? pair.Value.Selected
				: pair.Value.Idle;
		}
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
