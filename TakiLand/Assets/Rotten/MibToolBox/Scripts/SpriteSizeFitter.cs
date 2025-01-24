using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mib
{
	public class SpriteSizeFitter : MonoBehaviour
	{
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[Button]
		public void Fit(Sprite targetSprite)
		{
			_spriteRenderer.sprite = targetSprite;

			Camera mainCamera = Camera.main;

			float screenAspect = mainCamera!.aspect;
			float screenHeight = mainCamera.orthographicSize * 2;
			float screenWidth = screenAspect * screenHeight;

			float spriteHeight = targetSprite.bounds.size.y;
			float spriteWidth = targetSprite.bounds.size.x;
			float spriteAspect = spriteWidth / spriteHeight;

			float targetScale = screenAspect > spriteAspect
				? screenWidth / spriteWidth
				: screenHeight / spriteHeight;

			transform.localScale = Vector3.one * targetScale;
		}
	}
}