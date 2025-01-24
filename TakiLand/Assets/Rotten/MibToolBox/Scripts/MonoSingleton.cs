using UnityEngine;

namespace Mib
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		// ReSharper disable once StaticMemberInGenericType
		private static bool _hasInstance;

		public static T Instance
		{
			get
			{
				if (!_hasInstance)
				{
					_hasInstance = true;

					_instance = FindObjectOfType<T>();
					if (_instance == null)
					{
						var obj = new GameObject(typeof(T).ToString());
						_instance = obj.AddComponent<T>();
					}
				}

				return _instance;
			}
		}

		protected virtual bool IsPersistent => false;

		protected void Awake()
		{
			if (CheckAnotherInstance())
			{
				return;
			}

			// 접근이 없는(_instance == null인) singleton은 여기서 할당
			T _ = Instance;
			if (IsPersistent)
			{
				DontDestroyOnLoad(gameObject);
			}

			OnAwake();
		}

		protected virtual void OnDestroy()
		{
			if (_instance == this)
			{
				_instance = null;
				_hasInstance = false;
			}
		}

		protected abstract void OnAwake();

		private bool CheckAnotherInstance()
		{
			T[] objects = FindObjectsOfType<T>();
			int count = objects.Length;
			if (count >= 2)
			{
				foreach (T a in objects)
				{
					if (a != _instance)
					{
						Logg.Error($"Duplicated singleton object [{a.gameObject.name}] destoryed");
						Destroy(a.gameObject);
					}
				}

				return true;
			}

			return false;
		}
	}
	
	public abstract class SavableMonoSingleton<T, TV> : MonoSingleton<T> 
		where T : MonoBehaviour
		where TV : class, PointChecker.ISavableObject, new()
	{
		protected TV C
		{
			get;
			private set;
		}

		protected abstract void OnLoad(bool result);
		
		public void Clear()
		{
			PointChecker.Clear<TV>();
		}
		
		public void SaveData()
		{
			PointChecker.Save(C);
		}

		public bool LoadData()
		{
			bool result = PointChecker.Load(out TV c);
			C = c;
			return result;
		}

		public bool TryLoad()
		{
			bool result = LoadData();
			if (!result)
			{
				C = new TV();
			}

			OnLoad(result);
			return result;
		}
	}
}