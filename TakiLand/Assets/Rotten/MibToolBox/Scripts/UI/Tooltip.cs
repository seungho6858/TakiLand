using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Mib.UI
{
	public class Tooltip : MonoBehaviour, IPoolabeObject
	{
		[SerializeField]
		private TextMeshProUGUI _header;

		[SerializeField]
		private TextMeshProUGUI _body;

		private RectTransform _rectTransform;

		private void Awake()
		{
			_rectTransform = GetComponent<RectTransform>();
		}

		public void OnPull()
		{
		}

		public void OnPush()
		{
			_header.SetText(string.Empty);
			_body.SetText(string.Empty);
		}

		public void Set(RectTransform targetRect, string header, string body)
		{
			// 대충 가운데로..
			int pivotX = targetRect.position.x / Screen.width < 0.5f ? 0 : 1;
			int pivotY = targetRect.position.y / Screen.height < 0.5f ? 0 : 1;

			_rectTransform.pivot = new Vector2(pivotX, pivotY);
			_rectTransform.position = new Vector3(
				targetRect.position.x + targetRect.rect.size.x * 0.5f * (pivotX == 0 ? 1 : -1),
				targetRect.position.y + targetRect.rect.size.y * 0.5f * (pivotY == 0 ? 1 : -1));

			_header.SetText(header);
			_body.SetText(body);

			_header.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
			_body.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
		}
	}
}