namespace Mib
{
	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new T();
					_instance.OnInitialized();
					if (!_instance.IsPersistent())
					{
						SceneLoader.OnSceneChanging += _ => Reset();
					}
				}

				return _instance;
			}
		}

		protected abstract bool IsPersistent();
		protected abstract void OnInitialized();

		private static void Reset()
		{
			_instance = null;
		}
	}

	public abstract class SavableSingleton<T, TV> : Singleton<T> 
		where T : Singleton<T>, new() 
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

		public bool Initialize(bool willLoadData)
		{
			bool result = false;
			if (willLoadData)
			{
				result = LoadData();
			}
			
			if (!result)
			{
				C = new TV();
			}

			OnLoad(result);
			return result;
		}
	}
}