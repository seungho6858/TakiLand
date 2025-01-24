using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Mib.UI
{
	public class LoadingCurtain : MonoSingleton<LoadingCurtain>
	{
		[SerializeField]
		private Image _image;

		protected override bool IsPersistent => false;

		protected override void OnAwake()
		{
		}

		public void ShowImmediately()
		{
			_image.gameObject.SetActive(true);
			_image.color = Color.black;
		}

		public void HideImmediately()
		{
			_image.gameObject.SetActive(false);
			_image.color = Color.clear;
		}

		[Button]
		public async UniTask Show(float duration = 0.5f)
		{
			_image.gameObject.SetActive(true);
			_image.color = Color.clear;
			await _image.DOFade(1.0f, duration);
		}

		[Button]
		public async UniTask Hide(float duration = 0.5f)
		{
			_image.color = Color.black;
			await _image.DOFade(0.0f, duration);
			_image.gameObject.SetActive(false);
		}
	}
}