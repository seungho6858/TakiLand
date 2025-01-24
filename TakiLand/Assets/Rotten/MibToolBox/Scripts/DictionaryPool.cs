using System.Collections.Generic;
using UnityEngine;

namespace Mib
{
	public class DictionaryPool<TKey, TValue>
		where TKey : System.IEquatable<TKey>
		where TValue : Object
	{
		private readonly Dictionary<TKey, TValue> _dictionary;
		private readonly UnityObjectPool<TValue> _objectPool;
		
		public IReadOnlyDictionary<TKey, TValue> Dictionary => _dictionary;
		
		public int Count => _dictionary.Count;

		public TValue this[TKey key]
		{
			get => _dictionary[key];
			set => _dictionary[key] = value;
		}

		private DictionaryPool()
		{
		}

		private DictionaryPool(
			TValue baseObject,
			Transform parent,
			int defaultCapacity = 16,
			int initialCount = 4,
			bool useBase = false)
		{
			_dictionary = new Dictionary<TKey, TValue>(defaultCapacity);
			_objectPool = UnityObjectPool<TValue>.CreateInstance(baseObject, parent, defaultCapacity, initialCount, useBase);
		}

		public static DictionaryPool<TKey, TValue> CreateInstance(TValue baseObject, Transform parent, int defaultCapacity = 16, int initialCount = 4, bool useBase = false)
		{
			return new DictionaryPool<TKey, TValue>(baseObject, parent, defaultCapacity, initialCount, useBase);
		}
		

		public TValue Pull(TKey key)
		{
			TValue obj = _objectPool.Pull();
			_dictionary.Add(key, obj);
			return obj;
		}

		public void Push(TKey key)
		{
			TValue item = _dictionary[key];
			_objectPool.Push(item);
			_dictionary.Remove(key);
		}

		public void Clear()
		{
			foreach (var pair in _dictionary)
			{
				_objectPool.Push(pair.Value);
			}

			_dictionary.Clear();
		}

		public bool Contains(TKey item)
		{
			return _dictionary.ContainsKey(item);
		}
	}
}