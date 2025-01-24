using System.Collections.Generic;
using JetBrains.Annotations;

namespace Mib
{
	public class ObjectPool<T> where T : class, IPoolabeObject, new()
	{
		private readonly Stack<T> _pool = new();

		public T Pull()
		{
			T pulledObject = _pool.Count > 0 ? _pool.Pop() : new T();
			pulledObject.OnPull();
			return pulledObject;
		}

		public void Push(T poolableObject)
		{
			poolableObject.OnPush();
			_pool.Push(poolableObject);
		}
	}

	public class ObjectPools<TKey, TValue> where TValue : class, IPoolabeObject, new()
	{
		private readonly Dictionary<TKey, ObjectPool<TValue>> _pools = new ();

		public bool Contains(TKey key)
		{
			return _pools.ContainsKey(key);
		}

		public void CreatePool(TKey key)
		{
			if (Contains(key))
			{
				return;
			}

			_pools.Add(key, new ObjectPool<TValue>());
		}

		[NotNull]
		public TValue Pull(TKey key)
		{
			if (!_pools.TryGetValue(key, out ObjectPool<TValue> pool))
			{
				Logg.Error($"Has no pool for {key.ToString()}");
				return default!;
			}

			return pool.Pull();
		} 
		
		public void Push(TKey key, [NotNull] TValue item)
		{
			if (!_pools.TryGetValue(key, out ObjectPool<TValue> pool))
			{
				Logg.Error($"Has no pool for {key.ToString()}");
				return;
			}

			pool.Push(item);
		}		
	}
}