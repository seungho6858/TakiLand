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
	private Button _battleStartButton;
	
	[SerializeField]
	private TextMeshProUGUI _currentGoldText;
	
	private void Awake()
	{
		_battleStartButton.onClick.AddListener(() =>
		{
			SoundManager.PlaySound("Menu_Select_00");
			StageManager.Instance.BetDone(HasBet);
		});
		
		BettingManager.Instance.OnGoldChanged += (prev, current) =>
		{
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

	private ColorBlock _disabledColorBlock;
	private void Start()
	{
		_disabledColorBlock = _battleStartButton.colors;
		_disabledColorBlock.normalColor = _disabledColorBlock.disabledColor;
		_disabledColorBlock.highlightedColor = _disabledColorBlock.disabledColor;
		_disabledColorBlock.pressedColor = _disabledColorBlock.disabledColor;
		_disabledColorBlock.selectedColor = _disabledColorBlock.disabledColor;
	}

	// bettingManager에 상태만들기 귀찮아서..
	private bool HasBet => _battleStartButton.colors == ColorBlock.defaultColorBlock;

	private void CanBattleStart(bool value)
	{
		ColorBlock targetColorBlock = value ? ColorBlock.defaultColorBlock : _disabledColorBlock;
		_battleStartButton.colors = targetColorBlock;
	}
}
