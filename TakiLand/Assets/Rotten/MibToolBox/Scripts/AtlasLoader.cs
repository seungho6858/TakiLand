using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Mib
{
	public class AtlasLoader : MonoSingleton<AtlasLoader>
	{
		// async로 게임 시작시점에 쭉 부를지, 그냥 resource.load할지 고민
		private readonly Dictionary<string, SpriteAtlas> _cache = new Dictionary<string, SpriteAtlas>();

		// 콜백 등록을 클래스내에서 처리하게위해 dontdestroy함.
		// 메모리 부담이 있으면 해당 시점에 clear해주는게 좋을 듯
		// 아니면 씬 체인지 이벤트에 콜백으로 clear해주거나..
		protected override bool IsPersistent => true;

		protected override void OnAwake()
		{
			SpriteAtlasManager.atlasRequested += LoadAtlas;
		}

		protected override void OnDestroy()
		{
			SpriteAtlasManager.atlasRequested -= LoadAtlas;
			base.OnDestroy();
		}

		private void LoadAtlas(string atlasName, Action<SpriteAtlas> callback)
		{
			SpriteAtlas atlas = LoadAtlas(atlasName);
			callback.Invoke(atlas);
		}

		public void ClearCache()
		{
			_cache.Clear();
		}

		public SpriteAtlas LoadAtlas(string atlasName)
		{
			if (_cache.TryGetValue(atlasName, out SpriteAtlas result))
			{
				return result;
			}

			var spriteAtlas = Resources.Load<SpriteAtlas>($"Atlas/{atlasName}");
			_cache.Add(atlasName, spriteAtlas);
			return spriteAtlas;
		}

		public Sprite LoadSprite(string atlasName, string spriteName)
		{
			if (string.IsNullOrEmpty(atlasName) || string.IsNullOrEmpty(spriteName))
			{
				return null;
			}

			if (!_cache.TryGetValue(atlasName, out SpriteAtlas atlas))
			{
				atlas = LoadAtlas(atlasName);
			}

			return atlas.GetSprite(spriteName);
		}
	}
}