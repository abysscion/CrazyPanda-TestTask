using System.Collections;
using Attributes;
using UnityEngine;

namespace Tools.ObjectsPool
{
	public class PoolableMonoBehaviour : MonoBehaviour
	{
		[Header("Poolable settings")]
		[SerializeField, Tooltip("Delay until autorelease. Negative values means no autoreleasing.")]
		private float autoReleaseSecondsDelay = -1f;
		[SerializeField, ReadOnly] private int _prefabInstanceID;

		public int PoolId => _prefabInstanceID;

		protected virtual void Start()
		{
			PoolController.Instance.RegisterInstance(this, new PoolItemCallbacks(OnAcquireAction, OnReleaseAction));
			OnAcquireAction();
		}

		protected virtual void OnDestroy() => OnReleaseAction();

		private void OnValidate()
		{
			var prefabID = gameObject.GetInstanceID();
			if (prefabID > 0)
				_prefabInstanceID = prefabID;
		}

		protected virtual void OnAcquireAction()
		{
			if (autoReleaseSecondsDelay > 0f)
				StartCoroutine(ReleaseAfterDelay(autoReleaseSecondsDelay));
		}

		protected virtual void OnReleaseAction()
		{
			StopAllCoroutines();
		}

		protected virtual IEnumerator ReleaseAfterDelay(float secondsDelay)
		{
			yield return new WaitForSeconds(secondsDelay);
			PoolController.Instance.Release(this);
		}
	}
}