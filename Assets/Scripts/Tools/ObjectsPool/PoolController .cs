using System.Collections.Generic;
using Tools.Components;
using UnityEngine;

namespace Tools.ObjectsPool
{
	public class PoolController : MonoSingleton<PoolController>
	{
		[SerializeField] private Transform poolsRootTf;

		private readonly Dictionary<int, Pool<PoolableMonoBehaviour>> _prefabIdToPoolDic = new();
		private readonly Dictionary<int, PoolItemCallbacks> _instanceIdToCallbacksDic = new();

		public override void Initialize()
		{
			if (poolsRootTf)
				return;
			poolsRootTf = new GameObject("[ObjectPools]").transform;
		}

		public void RegisterInstance<T>(T instance, PoolItemCallbacks callbacks) where T : PoolableMonoBehaviour
		{
			var instanceId = instance.GetInstanceID();
			if (!_instanceIdToCallbacksDic.ContainsKey(instanceId))
				_instanceIdToCallbacksDic.Add(instanceId, callbacks);
		}

		public void Release<T>(T instance) where T : PoolableMonoBehaviour
		{
			if (!IsInstanceValid(instance))
				return;

			_instanceIdToCallbacksDic[instance.GetInstanceID()].OnRelease.Invoke();

			Pool<PoolableMonoBehaviour> pool = GetOrCreatePoolByInstance(instance);
			pool.Put(instance);
			instance.gameObject.SetActive(false);
			instance.transform.SetParent(pool.PoolRoot);
		}

		public T AcquireByPrefabAndSetup<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : PoolableMonoBehaviour
		{
			T instance = AcquireByPrefab(prefab);
			instance.transform.SetParent(parent ? parent : GetOrCreatePoolByInstance(prefab).PoolRoot);
			instance.transform.SetPositionAndRotation(position, rotation);
			return instance;
		}

		public T AcquireByPrefab<T>(T prefab) where T : PoolableMonoBehaviour
		{
			if (!IsInstanceValid(prefab))
				return null;

			PoolableMonoBehaviour instance = GetOrCreatePoolByInstance(prefab).Take();
			if (!instance)
			{
				return Instantiate(prefab);
			}
			else
			{
				instance.gameObject.SetActive(true);
				_instanceIdToCallbacksDic[instance.GetInstanceID()].OnAcquire.Invoke();
				return instance as T;
			}
		}

		public void ClearPools()
		{
			foreach (Pool<PoolableMonoBehaviour> pool in _prefabIdToPoolDic.Values)
				pool.Clear();
			_prefabIdToPoolDic.Clear();
		}

		private Pool<PoolableMonoBehaviour> GetOrCreatePoolByInstance<T>(T instance) where T : PoolableMonoBehaviour
		{
			if (!_prefabIdToPoolDic.TryGetValue(instance.PoolId, out Pool<PoolableMonoBehaviour> pool))
			{
				Transform poolRoot = new GameObject($"[{instance.name} Root]").transform;
				poolRoot.SetParent(poolsRootTf);
				_prefabIdToPoolDic.Add(instance.PoolId, pool = new Pool<PoolableMonoBehaviour>(instance.PoolId, poolRoot));
			}
			return pool;
		}

		private bool IsInstanceValid(PoolableMonoBehaviour instance)
		{
			if (!instance)
			{
				Debug.LogError($"[ObjectsPooling] Given instance is null!");
				return false;
			}
			if (instance.PoolId <= 0)
			{
				Debug.LogError($"[ObjectsPooling] Given object '{instance}' has no prefab? | PoolId: {instance.PoolId}");
				return false;
			}
			return true;
		}
	}
}