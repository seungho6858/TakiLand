using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NonDiegeticUI : MonoBehaviour
{
	[SerializeField]
	private Button _testFormationButton;
	
	[SerializeField]
	private Button _battleStartButton;
	
	[SerializeField]
	private TextMeshProUGUI _currentGoldText;
	
	private void Awake()
	{
		_testFormationButton.onClick.AddListener(() =>
		{
			StageManager.Instance.BettingProcess().Forget();
		});
		
		_battleStartButton.onClick.AddListener(() =>
		{
			StageManager.Instance.BetDone();
		});
		
		BettingManager.Instance.OnGoldChanged += (prev, current) =>
		{
			//TODO : 뭔가 애니메이션
			_currentGoldText.text = current.ToString();
		};

		StageManager.Instance.OnStageChanged += (_, _, _) =>
		{
			CanBattleStart(false);
		};

		BettingManager.Instance.OnBetTeamChanged += _ =>
		{
			CanBattleStart(true);
		};
	}

	private void CanBattleStart(bool value)
	{
		_battleStartButton.interactable = value;
	}
}
