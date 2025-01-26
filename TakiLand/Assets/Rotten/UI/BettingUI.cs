using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mib.Data;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BettingUI : MonoBehaviour, IPointerClickHandler
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
				SoundManager.PlaySound("Coin_push");
				BettingManager.Instance.BetTeam(betTeam);
			});
		}

		foreach (KeyValuePair<BetPreset, Button> pair in _betAmountButtons)
		{
			var preset = pair.Key;
			pair.Value.onClick.AddListener(() =>
			{
				SoundManager.PlaySound("Coin_push");
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
			Stage.Data stageData = Stage.Instance.Table[new Stage.Key(stage)];
			_currentRewardRate.text = $"x {stageData.RewardRate.ToString()}";
			_betRange.text = $"최소 {stageData.MinimumCost.ToString()} ~ 최대 {stageData.MaximumCost.ToString()}";
			
			SelectBetTeam(null);
			Show();
		};

		StageManager.Instance.OnBattleStart += () =>
		{
			Hide();
		};

		StageManager.Instance.OnBetFailed += () =>
		{
			HighlightButtons();
		};
	}

	private void HighlightButtons()
	{
		foreach (KeyValuePair<Team, Button> pair in _bettingButtons)
		{
			var background = pair.Value.GetComponent<Image>();
			Color targetColor = GeneralSetting.Instance.BetTeamColors[pair.Key];
			HighlightButtonAsync(background, Color.white, targetColor).Forget();
		}
	}
	
	private async UniTaskVoid HighlightButtonAsync(Image image, Color from, Color to)
	{
		const int delay = 100;
		
		image.color = to;
		await UniTask.Delay(delay, cancellationToken: this.GetCancellationTokenOnDestroy());
		image.color = from;
		await UniTask.Delay(delay, cancellationToken: this.GetCancellationTokenOnDestroy());
		image.color = to;
		await UniTask.Delay(delay, cancellationToken: this.GetCancellationTokenOnDestroy());
		image.color = from;
	}

	private void SelectBetTeam(Team? team)
	{
		foreach (var pair in GeneralSetting.Instance.TeamSprites)
		{
			Image target = _bettingButtonImages[pair.Key];
			target.sprite = team.HasValue && team.Value == pair.Key
				? pair.Value.Selected
				: pair.Value.Idle;
		}
		
		foreach (KeyValuePair<Team, Button> pair in _bettingButtons)
		{
			var background = pair.Value.GetComponent<Image>();
			background.color = team.HasValue && team.Value == pair.Key 
				? GeneralSetting.Instance.BetTeamColors[pair.Key]
				: Color.white;
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
	
	public void OnPointerClick(PointerEventData eventData)
	{
		transform.SetAsLastSibling();
	}
}
