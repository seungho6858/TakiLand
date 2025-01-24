using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mib.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mib
{
	public static class SceneLoader
	{
		public static event Action<string> OnSceneChanging;
		public static event Action<string> OnSceneChanged;

		public const string TITLE = "Title";
		public const string LOBBY = "Lobby";
		public const string MAIN = "Main";

		public static string CurrentScene => _current;

		private const string LOADING = "Loading";

		private static string _current = TITLE;
		private static readonly List<string> AdditiveScenes = new();
		private static bool _isLoading;
		private static bool _isAdditiveLoading = false;

		public static async UniTask LoadAdditiveScene(string sceneName)
		{
			await UniTask.WaitWhile(() => _isAdditiveLoading);
			
			if (AdditiveScenes.Contains(sceneName))
			{
				return;
			}

			_isAdditiveLoading = true;
			AdditiveScenes.Add(sceneName);
			await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			
			_isAdditiveLoading = false;
		}
		
		public static async UniTask UnloadAdditiveScene(string sceneName)
		{
			await UniTask.WaitWhile(() => _isAdditiveLoading);
			
			if (!AdditiveScenes.Contains(sceneName))
			{
				return;
			}

			_isAdditiveLoading = true;
			await SceneManager.UnloadSceneAsync(sceneName);
			AdditiveScenes.Remove(sceneName);
			
			_isAdditiveLoading = false;
		}

		public static async UniTaskVoid ChangeScene(string to, bool skipCurtain = false)
		{
			if (_isLoading)
			{
				Logg.Error("now changing!!");
				return;
			}

			_isLoading = true;

			OnSceneChanging?.Invoke(_current);

			// 타겟 열기
			// operation.isAllow~~이게 하나 걸어놓으면 모든 SceneManager동작(load, unload)에 다 영향을 줘서
			// 다른 씬 열닫도 제대로 동작하지 않게 된다. 전역변수처럼 동작함.
			// 아무래도 버그같음. 뭐 이따위로 만들었어...
			// 일단은 순차 로딩으로 간다. 이러면 로딩씬을 따로 쓰는의미가 없지만..
			//var operation =  SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
			//operation.allowSceneActivation = false;

			// 로딩 열고 보여주기
			await SceneManager.LoadSceneAsync(LOADING, LoadSceneMode.Additive);
			if (!skipCurtain)
			{
				await LoadingCurtain.Instance.Show();
			}

			// 이전꺼 닫기
			Scene loadingScene = SceneManager.GetSceneByName(LOADING);
			SceneManager.SetActiveScene(loadingScene);
			await SceneManager.UnloadSceneAsync(_current);

			// 타겟 열기 TODO: 나중에 저 버그 해결되면 여기 제거하고 위에 주석 풀것.
			AsyncOperation operation = SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);

			// 타겟 보여줌
			//operation.allowSceneActivation = true;
			await operation;
			Scene targetScene = SceneManager.GetSceneByName(to);
			SceneManager.SetActiveScene(targetScene);

			_current = to;

			OnSceneChanged?.Invoke(_current);
		}

		// 씬 전환되고 커튼 걷혀야하는 시점에 수동 호출해줘야함.
		public static async UniTaskVoid CompleteLoading()
		{
			// 로딩 닫기
			await LoadingCurtain.Instance.Hide();
			await SceneManager.UnloadSceneAsync(LOADING);

			_isLoading = false;
		}
	}
}