using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Mib.UI
{
	public class TooltipManager : MonoSingleton<TooltipManager>
	{
		private const int UNIQUE_KEY = 0; // InstanceId는 0이 될 수 없음.

		[SerializeField]
		private Tooltip _tooltipPrefab;

		private readonly Dictionary<int, AutoResetUniTaskCompletionSource> _currentTooltips = new();

		private UnityObjectPool<Tooltip> _tooltipPool;

		protected override void OnAwake()
		{
			_tooltipPool = UnityObjectPool<Tooltip>.CreateInstance(_tooltipPrefab, transform);
		}

		public async UniTaskVoid ShowTooltip(RectTransform targetRect, string header, string body,
			float duration = 10.0f, bool isUnique = true)
		{
			if (duration <= 0.0f)
			{
				Logg.Error($"Invalid tooltip duration {duration.ToString(CultureInfo.InvariantCulture)}");
				return;
			}

			int key = isUnique ? UNIQUE_KEY : targetRect.GetHashCode();
			if (_currentTooltips.TryGetValue(key, out AutoResetUniTaskCompletionSource existTcs))
			{
				existTcs.TrySetResult();
				if (!isUnique)
				{
					return;
				}
			}

			Tooltip tooltip = _tooltipPool.Pull(transform);
			tooltip.Set(targetRect, header, body);
			tooltip.transform.SetAsLastSibling();

			var tcs = AutoResetUniTaskCompletionSource.Create();

			_currentTooltips.Add(key, tcs);

			await UniTask.WhenAny(tcs.Task, UniTask.WaitForSeconds(duration));

			_currentTooltips.Remove(key);
			_tooltipPool.Push(tooltip);
		}
	}
}