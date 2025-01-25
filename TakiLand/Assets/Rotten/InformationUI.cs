using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mib.Data;
using TMPro;
using UnityEngine;

public class InformationUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _currentStageText;
	
	[SerializeField]
	private TextMeshProUGUI _remainTimeText;
	
	[SerializeField]
	private SerializableDictionary<Team, TextMeshProUGUI> _remainUnitCount;

	private void Awake()
	{
		_currentStageText.text = "";
		_remainTimeText.text = "";
		
		BattleManager.onBattleTimer += remainTime =>
		{
			_remainTimeText.text = remainTime.ToString("N0");
		};

		StageManager.Instance.OnStageChanged += (teamRed, teamBlue, stage) =>
		{
			var redTeamCount = teamRed.Positions.Count(action => action != SpecialAction.None);
			var blueTeamCount = teamBlue.Positions.Count(action => action != SpecialAction.None);
			
			UpdateSlimeCount(redTeamCount, blueTeamCount);
			_currentStageText.text = $"Round {stage.ToString()}";
		};

		BattleManager.OnTeamCountChanged += (teamRed, teamBlue) =>
		{
			UpdateSlimeCount(teamRed, teamBlue);
		};
	}

	private void UpdateSlimeCount(int redCount, int blueCount)
	{
		_remainUnitCount[Team.Red].text = redCount.ToString();
		_remainUnitCount[Team.Blue].text = blueCount.ToString();
	}
}
