using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EnumsNET;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Mib.UI
{
	public class PopupManager : MonoSingleton<PopupManager>
	{
		[SerializeField]
		private Image _background;

		private readonly Dictionary<string, PopupBase> _popupPool = new();

		private (PopupBase popup, AutoResetUniTaskCompletionSource tcs) _current;

		private readonly Stack<(PopupBase popup, AutoResetUniTaskCompletionSource tcs)> _popupStack = new();

		public bool AnyOpened => _current.popup != null;

		protected override void OnAwake()
		{
			_background.gameObject.SetActive(false);
		}

		public (T, UniTask) OpenWithTask<T>() where T : PopupBase
		{
			string path = GetFilePath(typeof(T));
			var popup = OpenInternal(path) as T;
			var tcs = AutoResetUniTaskCompletionSource.Create();
			_current = (popup, tcs);

			return (popup, tcs.Task);
		}

		public T Open<T>() where T : PopupBase
		{
			string path = GetFilePath(typeof(T));
			var popup = OpenInternal(path) as T;
			_current = (popup, null);

			return popup;
		}

		public PopupBase Open(Type popupType)
		{
			string path = GetFilePath(popupType);
			PopupBase popup = OpenInternal(path);
			_current = (popup, null);

			return popup;
		}

		private PopupBase OpenInternal(string path)
		{
			if (AnyOpened)
			{
				if (_current.popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.Stackable))
				{
					bool isHidden = _current.popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.CloseOnStack);
					
					_current.popup.gameObject.SetActive(!isHidden);
					_popupStack.Push(_current);
				}
				else
				{
					CloseCurrent();
				}
			}
			
			if (_popupPool.TryGetValue(path, out PopupBase popup))
			{
				popup.transform.SetAsLastSibling();
				popup.gameObject.SetActive(true);
			}
			else
			{
				var result = Resources.Load(path) as GameObject;
				GameObject go = Instantiate(result, transform);
				popup = go.GetComponent<PopupBase>();
				_popupPool.Add(path, popup);
			}

			if (popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.HasBackground))
			{
				int targetIndex = popup.transform.GetSiblingIndex() - 1;
				_background.transform.SetSiblingIndex(targetIndex);
				_background.GameObject().SetActive(true);
			}

			popup.OnOpen();
			return popup;
		}

		private static string GetFilePath(Type type)
		{
			foreach (object customAttribute in type.GetCustomAttributes(true))
			{
				if (customAttribute is PopupPathAttribute attribute)
				{
					return attribute.Path;
				}
			}

			return string.Empty;
		}

		public void OnBackgroundClicked()
		{
			if (_current.popup != null &&
			    _current.popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.InteractableBackground) &&
			    !_current.popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.NonClosable))
			{
				CloseCurrent();
			}
		}

		public void CloseCurrent()
		{
			if (_current.popup == null)
			{
				Logg.Error("there is nopopup");
				return;
			}

			if (_current.popup.PopupAttribute.HasAnyFlags(PopupBase.Attribute.HasBackground))
			{
				_background.gameObject.SetActive(false);
			}

			_current.popup.Close();
			AutoResetUniTaskCompletionSource tcs = _current.tcs;
			if (_popupStack.TryPop(out _current))
			{
				_current.popup.gameObject.SetActive(true);
			}
			else
			{
				_current = (null, null);
			}

			if (tcs != null)
			{
				tcs.TrySetResult();
			}
		}
	}
}