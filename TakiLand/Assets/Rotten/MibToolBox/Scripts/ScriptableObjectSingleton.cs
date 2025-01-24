using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mib
{
	public abstract class ScriptableObjectSingleton<T> : SerializedScriptableObject where T : SerializedScriptableObject
	{
		private static T _instance;

		// ReSharper disable once StaticMemberInGenericType
		private static bool _hasValue;

		public static T Instance
		{
			get
			{
#if UNITY_EDITOR

				// 이전 데이터를 가진 instance가 유지되어
				// Data Loader가 validate시 오동작 하는 것을 막기위함  
				if (!Application.isPlaying)
				{
					_hasValue = false;
				}
#endif

				if (_hasValue)
				{
					return _instance;
				}

				if (_instance == null)
				{
					string path = GetFilePath();
					var comp = Resources.Load<T>(path);
					if (comp == null)
					{
						Debug.LogError($"InvalidPath: [{path}]");
						return null;
					}

					_hasValue = true;
					_instance = comp;
				}

				return _instance;
			}
		}

		private static string GetFilePath()
		{
			foreach (object customAttribute in typeof(T).GetCustomAttributes(true))
			{
				if (customAttribute is ScriptableObjectPathAttribute attribute)
				{
					return attribute.Path;
				}
			}

			return string.Empty;
		}
	}

	public class ScriptableObjectPathAttribute : Attribute
	{
		public string Path
		{
			get;
			private set;
		}

		public ScriptableObjectPathAttribute(string path)
		{
			Path = path;
		}
	}
}