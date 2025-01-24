using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mib.UI
{
	public interface IInfoIconable
	{
		Sprite GetIcon();
		string GetSubject();
		string GetContent();
	}

	public class InfoIcon : MonoBehaviour, IPointerClickHandler, IPoolabeObject
	{
		[SerializeField]
		private Image _icon;

		[SerializeField]
		private TextMeshProUGUI _subject;

		[SerializeField]
		private TextMeshProUGUI _content;

		[SerializeField]
		private TextMeshProUGUI _level;

		[SerializeField]
		private TextMeshProUGUI _count;

		private TextMeshProUGUI _initialContent;
		private TextMeshProUGUI _initialCount;
		private TextMeshProUGUI _initialLevel;
		private TextMeshProUGUI _initialSubject;

		private Action<IInfoIconable> _onClick;

		public IInfoIconable Data
		{
			get;
			private set;
		}

		private void Awake()
		{
			_initialSubject = _subject;
			_initialContent = _content;
			_initialLevel = _level;
			_initialCount = _count;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (_onClick != null)
			{
				_onClick.Invoke(Data);
				return;
			}

			var rect = (RectTransform)transform;
			TooltipManager.Instance
				.ShowTooltip(rect, Data.GetSubject(), Data.GetContent(), 3.0f)
				.Forget();
		}

		public void OnPull()
		{
			if (_subject != null)
			{
				_subject.SetText(string.Empty);
			}

			if (_content != null)
			{
				_content.SetText(string.Empty);
			}

			if (_level != null)
			{
				_level.SetText(string.Empty);
			}

			if (_count != null)
			{
				_count.SetText(string.Empty);
			}
		}

		public void OnPush()
		{
			Data = null;
			_onClick = null;
			_icon.sprite = null;
			SetFromOuterScope(_initialSubject, _initialContent, _initialLevel, _initialCount);
		}

		public InfoIcon SetFromOuterScope([CanBeNull] TextMeshProUGUI subject = null,
			[CanBeNull] TextMeshProUGUI content = null, [CanBeNull] TextMeshProUGUI level = null,
			[CanBeNull] TextMeshProUGUI count = null)
		{
			_subject = subject == null ? _initialSubject : subject;
			_content = content == null ? _initialContent : content;
			_level = level == null ? _initialLevel : level;
			_count = count == null ? _initialCount : count;

			return this;
		}

		public InfoIcon SetAnchorToFit(RectTransform target)
		{
			var rect = transform as RectTransform;

			rect.anchorMin = Vector2.one * 0.5f;
			rect.anchorMax = Vector2.one * 0.5f;

			rect.anchoredPosition = Vector2.zero;
			rect.sizeDelta = new Vector2(rect.sizeDelta.x, target.sizeDelta.y);

			return this;
		}

		public InfoIcon SetOnClick(Action<IInfoIconable> callback)
		{
			_onClick = callback;
			return this;
		}

		public InfoIcon SetData(IInfoIconable data)
		{
			Data = data;
			_icon.sprite = Data.GetIcon();

			if (_subject != null)
			{
				_subject.SetText(data.GetSubject());
			}

			if (_content != null)
			{
				_content.SetText(data.GetContent());
			}

			return this;
		}

		public InfoIcon SetLevel(int level)
		{
			if (level >= 0 && _level != null)
			{
				_level.SetText(level.ToString());
			}

			return this;
		}

		public InfoIcon SetCount(int count)
		{
			if (count >= 0 && _count != null)
			{
				_count.SetText(count.ToString());
			}

			return this;
		}
	}
}