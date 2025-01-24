using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mib.UI
{
	public class ClickableText : MonoBehaviour, IPointerClickHandler
	{
		private TextMeshProUGUI _text;

		private void Awake()
		{
			_text = GetComponent<TextMeshProUGUI>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, Input.mousePosition, null);

			TMP_LinkInfo linkInfo = linkIndex != -1
				? _text.textInfo.linkInfo[linkIndex]
				: default;

			Logg.Error($"clicked!! {linkIndex} {linkInfo.GetLinkID()}");
		}
	}
}