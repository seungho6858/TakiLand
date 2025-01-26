using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mib;
using Mib.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class Title : MonoBehaviour
{
	[SerializeField]
	private Button _playButton;
	
	[SerializeField]
	private Button _creditButton;
	
	[SerializeField]
	private Button _exitButton;

	private void Awake()
	{
		// 프레임 60 고정 
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		
		_playButton.onClick.AddListener(() =>
		{
			SoundManager.PlaySound("Menu_Select_00");
			SceneLoader.ChangeScene(Constant.BattleScene).Forget();
		});
		
		_creditButton.onClick.AddListener(() =>
		{
			Mib.UI.PopupManager.Instance.Open<CreditPopup>();
			SoundManager.PlaySound("Menu_Select_00");
		});
		
		_exitButton.onClick.AddListener(() =>
		{
			Mib.UI.PopupManager.Instance.Open<Popup_Ranking>();
			SoundManager.PlaySound("Menu_Select_00");
		});
		
		SoundManager.PlayLoopSound("track_shortadventure_loop");
	}

	private void Start()
	{
		SceneLoader.CompleteLoading().Forget();
	}
}
