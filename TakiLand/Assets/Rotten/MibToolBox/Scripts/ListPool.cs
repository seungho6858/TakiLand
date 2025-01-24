using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mib
{
	public class ListPool<T> where T : Object
	{
		private readonly List<T> _list;
		private readonly UnityObjectPool<T> _objectPool;

		public IReadOnlyList<T> List => _list;

		public int Count => _list.Count;

		public T this[int index]
		{
			get => _list[index];
			set => _list[index] = value;
		}

		private ListPool(
			T baseObject,
			Transform parent,
			int defaultCapacity = 16,
			int initialCount = 4,
			bool useBase = false)
		{
			_list = new List<T>(defaultCapacity);
			_objectPool = UnityObjectPool<T>.CreateInstance(baseObject, parent, defaultCapacity, initialCount, useBase);
		}

		public static ListPool<T> CreateInstance(T baseObject, Transform parent, int defaultCapacity = 16, int initialCount = 4, bool useBase = false)
		{
			return new ListPool<T>(baseObject, parent, defaultCapacity, initialCount, useBase);
		}

		public T Pull()
		{
			T obj = _objectPool.Pull();
			_list.Add(obj);
			return obj;
		}

		public bool Push(T item)
		{
			bool result = _list.Remove(item);
			if (result)
			{
				_objectPool.Push(item);
			}

			return result;
		}

		public void Push(int index)
		{
			T item = _list[index];
			_objectPool.Push(item);
			_list.RemoveAt(index);
		}

		public void Clear()
		{
			foreach (T poolableObject in _list)
			{
				_objectPool.Push(poolableObject);
			}

			_list.Clear();
		}

		public bool Contains(T item)
		{
			return _list.Contains(item);
		}

		public int IndexOf(T item)
		{
			return _list.IndexOf(item);
		}
	}
}