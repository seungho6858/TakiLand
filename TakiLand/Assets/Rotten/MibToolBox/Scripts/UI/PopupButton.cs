 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mib.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mib.UI
{
	public class PopupButton : MonoBehaviour, IPointerClickHandler
	{
		[ValueDropdown("GetPopupTypes")]
		[SerializeField]
		private string _popupType;

		private static IEnumerable GetPopupTypes()
		{
			var assembly = Assembly.GetExecutingAssembly();
			Type[] allTypes = assembly.GetTypes();
			return allTypes
				.Where(type => type.IsSubclassOf(typeof(PopupBase)))
				.Select(type => type.ToString());
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			PopupManager.Instance.Open(Type.GetType(_popupType));
		}
	}
}