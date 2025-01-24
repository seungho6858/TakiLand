using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mib.Effect
{
	public class ParticleEffect : EffectBase
	{
		[InfoBox("파티클 시스템이 없거나 Looping인 경우 설정된 시간만큼만 재생")]
		[DisableIf("HasParticleSystem")]
		[SerializeField]
		private float _playTime;

		private ParticleSystem _particleSystem;

		private void Awake()
		{
			_particleSystem = GetComponent<ParticleSystem>();
		}

		[InfoBox("자식중에 particleSystem이 있으면 가장 긴 duration을 가져와 세팅.")]
		[DisableIf("HasParticleSystem")]
		[Button]
		private void UpdatePlayTime()
		{
			ParticleSystem[] psArray = GetComponentsInChildren<ParticleSystem>();
			float longestDuration = psArray.Max(system => system.main.duration);
			_playTime = longestDuration;
		}

		[UsedImplicitly]
		public bool HasParticleSystem()
		{
			var ps = GetComponent<ParticleSystem>();
			return ps != null && !ps.main.loop;
		}

		protected override async UniTask OnPlay(Vector3 sourcePosition, Vector3 targetPosition, float scale)
		{
			transform.localScale = Vector3.one * scale;
			
			UpdateTransform(sourcePosition, targetPosition);
			
			if (_particleSystem != null)
			{
				_particleSystem.Play();
			}

			float playTime = _playTime;
			if (playTime > 0.0f || _particleSystem == null)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(playTime),
					cancellationToken: this.GetCancellationTokenOnDestroy());

				return;
			}

			await UniTask.WaitWhile(() => _particleSystem.isPlaying,
				cancellationToken: this.GetCancellationTokenOnDestroy());
		}

		public override UniTask Play(ILocatable source, ILocatable target, float scale)
		{
			base.Play(source, target, scale);
			return OnPlay(source.Position(), target.Position(), scale);
		}

		protected override void UpdateTransform(Vector3 source, Vector3 target)
		{
			transform.position = target;
		}
	}
}