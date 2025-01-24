using Sirenix.OdinInspector;
using UnityEngine;

namespace Mib.UI
{
	public class SpatialUIBase : MonoBehaviour
	{
		[SerializeField]
		private Vector2 _offset;

		private Vector3 _prevPosition;

		protected Transform TargetTransform
		{
			private get;
			set;
		}

		protected virtual void Update()
		{
			if (_prevPosition != TargetTransform.position)
			{
				UpdatePosition();
			}
		}

		[Button]
		protected void UpdatePosition()
		{
			_prevPosition = TargetTransform.position;

			Vector2 screenPoint = Camera.main.WorldToScreenPoint(_prevPosition);
			var ratio = new Vector2(
				(float)Screen.width / Constant.DEFAULT_WIDTH,
				(float)Screen.height / Constant.DEFAULT_HEIGHT);

			transform.position = screenPoint + _offset * ratio;
		}
	}
}