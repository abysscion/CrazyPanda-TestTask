using System.Collections.Generic;
using Attributes;
using UnityEngine;

namespace Tools.ObjectsPool
{
	[System.Serializable]
	public class Pool<T> where T : Object
	{
		[SerializeField, ReadOnly] private int originPrefabId;
		[SerializeField, ReadOnly] private Transform poolRoot;
		[SerializeField, ReadOnly] private List<T> items = new();

		public Transform PoolRoot => poolRoot;

		public Pool(int originPrefabId, Transform poolRoot)
		{
			this.originPrefabId = originPrefabId;
			this.poolRoot = poolRoot;
		}

		public T Take()
		{
			while (items.Count > 0)
			{
				var i = items.Count - 1;
				T item = items[i];
				items.RemoveAt(i);
				if (item)
					return item;
			}
			return null;
		}

		public void Put(T obj)
		{
			items.Add(obj);
		}

		public void Clear()
		{
			foreach (T item in items)
			{
				if (item)
					Object.Destroy(item);
			}

			items.Clear();
		}
	}
}