//#define ADDRESSABLE

using System;
using System.Collections.Generic;
using Cysharp.Text;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

#if ADDRESSABLE
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
#endif

namespace Mib
{
	public class UnityObjectPool<T> where T : Object
	{
		private readonly Stack<T> _pool;
		private readonly T _baseObject;
		private readonly Transform _owner;

		private UnityObjectPool()
		{
		}

		private UnityObjectPool(T baseObject, Transform owner, int defaultCapacity, int initialCount, bool useBase)
		{
			_baseObject = Object.Instantiate(baseObject, owner);
			_baseObject.name = baseObject.name;
			_owner = owner;
			_baseObject.Go().SetActive(false);
			_pool = new Stack<T>(defaultCapacity);
			if (useBase)
			{
				if (_baseObject is IPoolabeObject poolable)
				{
					poolable.OnPull();
				}

				_pool.Push(_baseObject);
			}

			for (int i = _pool.Count; i < initialCount; ++i)
			{
				T item = Pull(_owner, forceNew: true);
				Push(item);
			}
		}

		public static UnityObjectPool<T> CreateInstance(T baseObject, Transform owner, int defaultCapacity = 16,
			int initialCount = 4,
			bool useBase = false)
		{
			return new UnityObjectPool<T>(baseObject, owner, defaultCapacity, initialCount, useBase);
		}

#if ADDRESSABLE
		public static async UniTask<UnityObjectPool<T>> CreateInstance(string addressableName, Transform owner,
			int defaultCapacity = 16, int initialCount = 4, bool useBase = false)
		{
			GameObject gameObject = await Addressables.InstantiateAsync(addressableName, owner);
			if (gameObject == null)
			{
				Logg.Error($"Invalid address: {addressableName}");
			}

			var component = gameObject.GetComponent<T>();
			return new UnityObjectPool<T>(component, owner, defaultCapacity, initialCount, useBase);
		}
#endif

		public T Pull(int order = -1, bool forceNew = false)
		{
			return Pull(_baseObject.Tr().parent, order, forceNew);
		}
		
		public T Pull(Transform parent, int order = -1, bool forceNew = false)
		{
			T item;
			if (_pool.Count > 0 && !forceNew)
			{
				item = _pool.Pop();

				Transform transform = item.Tr();
				Transform baseTransform = _baseObject.Tr();

				transform.SetParent(parent);
				transform.localPosition = baseTransform.localPosition;
				transform.localRotation = baseTransform.localRotation;
				transform.localScale = baseTransform.localScale;
			}
			else
			{
				item = Object.Instantiate(_baseObject, parent);
				item.Go().name = ZString.Format("{0}_{1}", _baseObject.name, Guid.NewGuid());
			}

			if (order >= 0)
			{
				item.Tr().SetSiblingIndex(order);
			}

			item.Go().SetActive(true);
			if (item is IPoolabeObject poolable)
			{
				poolable.OnPull();
			}

			return item;
		}

		public void Push(T item)
		{
			if (item is IPoolabeObject poolable)
			{
				poolable.OnPush();
			}

			item.Go().SetActive(false);
			item.Tr().SetParent(_owner);

			_pool.Push(item);
		}

		public void DestroyBaseObject()
		{
			Object.Destroy(_baseObject.Go());
		}
	}

	public class UnityObjectPools<TKey, TValue> where TValue : Object
	{
		private readonly Dictionary<TKey, UnityObjectPool<TValue>> _pools;
		private readonly Transform _owner;

		private UnityObjectPools()
		{
		}

		public UnityObjectPools(Transform parent, int capacity = 16)
		{
			_owner = parent;
			_pools = new Dictionary<TKey, UnityObjectPool<TValue>>(capacity);
		}

		public bool Contains(TKey key)
		{
			return _pools.ContainsKey(key);
		}

#if ADDRESSABLE
		public async UniTask CreatePool(TKey key, string path, int capacity = 16, int initialCount = 4,
			bool useBase = true)
		{
			if (Contains(key))
			{
				return;
			}

			UnityObjectPool<TValue> pool =
				await UnityObjectPool<TValue>.CreateInstance(path, _owner, capacity, initialCount, useBase);

			// 동시에 생성요청이 들어왔을 경우, 하나는 지움
			// 이걸 tcs로 빼서 해봤는데, task 두 개 이상이 대기하는 경우 어차피 망하기 때문에ㅜ 걍 생성 후 삭제로 바꿈.
			if (Contains(key))
			{
				pool.DestroyBaseObject();
				Logg.Warn("Trying to making 2 same pool at the same time. The last one will be deleted");
				return;
			}

			_pools.Add(key, pool);
		}
#endif

		public void CreatePool(TKey key, TValue item, int capacity = 16, int initialCount = 4, bool useBase = false)
		{
			if (Contains(key))
			{
				return;
			}

			UnityObjectPool<TValue> pool =
				UnityObjectPool<TValue>.CreateInstance(item, _owner, capacity, initialCount, useBase);

			_pools.Add(key, pool);
		}

		[NotNull]
		public T Pull<T>(TKey key, Transform parent, int order = -1) where T : TValue
		{
			if (!_pools.TryGetValue(key, out UnityObjectPool<TValue> pool))
			{
				Logg.Error($"Has no pool for {key.ToString()}");
				return default!;
			}

			return (pool.Pull(parent, order) as T)!;
		} // ReSharper disable Unity.PerformanceAnalysis
		public void Push(TKey key, [NotNull] TValue item)
		{
			if (!_pools.TryGetValue(key, out UnityObjectPool<TValue> pool))
			{
				Logg.Error($"Has no pool for {key.ToString()}");
				return;
			}

			pool.Push(item);
		}
	}
}