using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mib.Effect
{
	public class Stretch : EffectBase
	{
		[SerializeField]
		private float _duration;
		
		private LineRenderer _lineRenderer;
		private bool _hasLineRenderer;

		[SerializeField]
		private GameObject _startEffect;
		[SerializeField]
		private GameObject _endEffect;
		private void Awake()
		{
			_hasLineRenderer = TryGetComponent(out _lineRenderer);
		}

		protected override void UpdateTransform(Vector3 source, Vector3 target)
		{
			if( _startEffect != null) _startEffect.transform.position = source;
			if( _endEffect != null) _endEffect.transform.position = target;
			
			if (_hasLineRenderer)
			{
				_lineRenderer.positionCount = 2;
				_lineRenderer.SetPosition(0, source);
				_lineRenderer.SetPosition(1, target);

			}
			else
			{
				Transform tr = transform;
				tr.position = source;
				tr.LookAt(target);
				tr.localScale = new Vector3(tr.localScale.x, tr.localScale.y, (target - source).magnitude);
				
			}
		}

		protected override UniTask OnPlay(Vector3 sourcePosition, Vector3 targetPosition, float scale)
		{
			transform.localScale = Vector3.one * scale;
			UpdateTransform(sourcePosition, targetPosition);
			return UniTask.WaitForSeconds(_duration);
		}

		public override UniTask Play(ILocatable source, ILocatable target, float scale)
		{
			base.Play(source, target, scale);
			return UniTask.WhenAny(
				UniTask.WaitWhile(() => source.IsValid() && target.IsValid()),
				UniTask.WaitForSeconds(_duration));
		}
	}
}