using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Mib.Effect
{
    public interface ILocatable
    {
        Vector3 Position();
        bool IsValid();
    }
    
	public abstract class EffectBase : MonoBehaviour, IPoolabeObject
	{
		protected ILocatable Target;
		protected ILocatable Origin;

		[SerializeField]
		protected AudioClip _sound;

		private void Update()
		{
			if (Origin != null && Target != null)
			{
				UpdateTransform(Origin.Position(), Target.Position());
			}
		}

		public void OnPull()
		{
			Target = null;
			Origin = null;
			transform.localScale = Vector3.one;
		}

		public void OnPush()
		{
		}

		public UniTask Play(Vector3 sourcePosition, Vector3 targetPosition, float scale)
		{
			if (_sound != null)
			{
				SfxManager.Instance.PlaySfx(_sound).Forget();
			}

			return OnPlay(sourcePosition,targetPosition, scale);
		}

		protected abstract UniTask OnPlay(Vector3 sourcePosition, Vector3 targetPosition, float scale);

		public virtual UniTask Play([NotNull] ILocatable source, [NotNull] ILocatable target, float scale)
		{
			if (_sound != null)
			{
				SfxManager.Instance.PlaySfx(_sound).Forget();
			}
			
			Target = source;
			Origin = target;
			transform.localScale = Vector3.one * scale;
			
			return UniTask.CompletedTask;
		}

		protected abstract void UpdateTransform(Vector3 source, Vector3 target);
	}
}