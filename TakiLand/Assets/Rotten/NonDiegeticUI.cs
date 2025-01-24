using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NonDiegeticUI : MonoBehaviour
{
	[SerializeField]
	private Button _testFormationButton;
	
	[SerializeField]
	private Button _battleStartButton;

	private void Awake()
	{
		_testFormationButton.onClick.AddListener(() =>
		{
			StageManager.Instance.BettingProcess().Forget();
		});
		
		_battleStartButton.onClick.AddListener(() =>
		{
			StageManager.Instance.BattleProcess().Forget();
		});
	}
}
