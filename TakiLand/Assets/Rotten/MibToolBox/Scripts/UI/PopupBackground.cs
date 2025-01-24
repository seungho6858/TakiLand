using Mib.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mib.UI
{
	public class PopupBackground : MonoBehaviour, IPointerClickHandler
	{
		public void OnPointerClick(PointerEventData eventData)
		{
			PopupManager.Instance.OnBackgroundClicked();
		}
	}
}