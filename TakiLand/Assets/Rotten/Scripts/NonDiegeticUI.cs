using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class NonDiegeticUI : MonoBehaviour
{
	[SerializeField]
	private Button _battleStartButton;

	private void Awake()
	{
		_battleStartButton.onClick.AddListener(() =>
		{
			Debug.Log("Battle Start");
			StageManager.Instance.BattingProcess().Forget();
		});
	}
}
