using System;
using Cysharp.Threading.Tasks;
using EnumsNET;
using UnityEngine;
using UnityEngine.UI;

namespace Mib.UI
{
	public abstract class PopupBase : MonoBehaviour
	{
		[Flags]
		public enum Attribute
		{
			None = 0,
			NonClosable = 1 << 0,
			InteractableBackground = 1 << 1, // 백그라운드 터치시 닫힘
			Stackable = 1 << 2, // 스택커블이면 누적되어 쌓임. latest를 닫는 경우에 그 전꺼가 보여짐. false면 다음 팝업이 열릴때 사라짐
			CloseOnStack = 1 << 3, // 스택인데 닫힘
			// Duplicatable = 1 << 3, //미구현, Stackable한 pupop이 중복해서 나올 때 체크.
			// (한 팝업을 돌려쓰면 먼저 스택된 팝업이 꼬일 수 있으므로 별도로 구현해줘야함,
			// 사실상 notunique인데 대부분이 unique이므로 duplicatable을 만듦)  
			HasBackground = 1 << 4,
		}

		[SerializeField]
		private Button _closeButton;

		[SerializeField]
		private Attribute _attribute;

		public Attribute PopupAttribute => _attribute;

		private void Awake()
		{
			if (_closeButton != null && !PopupAttribute.HasAnyFlags(Attribute.NonClosable))
			{
				_closeButton.onClick.AddListener(PopupManager.Instance.CloseCurrent);
			}

			OnAwake();
		}

		public void Close()
		{
			OnClose();
			gameObject.SetActive(false);
		}

		protected abstract void OnAwake();
		public abstract void OnOpen();
		protected abstract void OnClose();
	}

	public class PopupPathAttribute : Attribute
	{
		public string Path
		{
			get;
			private set;
		}

		public PopupPathAttribute(string path)
		{
			Path = path;
		}
	}
}