using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mib.Effect
{
	public class Projectile : EffectBase
	{
		protected override UniTask OnPlay(Vector3 sourcePosition, Vector3 targetPosition, float scale)
		{
			Logg.Error("Projectile Cannot Be Played with position");
			return UniTask.CompletedTask;
		}

		public override UniTask Play(ILocatable source, ILocatable target, float scale)
		{
			base.Play(source, target, scale);
			return UniTask.WaitWhile(() => Target.IsValid());
		}

		protected override void UpdateTransform(Vector3 source, Vector3 target)
		{
			Transform tr = transform;
			tr.LookAt(target);
			tr.position = target;
		}
	}
}