using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Mib;
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
		_playButton.onClick.AddListener(() =>
		{
			SceneLoader.ChangeScene(Constant.BattleScene).Forget();
		});
		
		_creditButton.onClick.AddListener(() =>
		{
			// TODO: creadit만들기
		});
		
		_exitButton.onClick.AddListener(() =>
		{
			SceneLoader.ChangeScene(Constant.BattleScene).Forget();
			// TODO: 페이크 버튼 만들기
		});
		
		SoundManager.PlayLoopSound("track_shortadventure_loop");
	}
}
