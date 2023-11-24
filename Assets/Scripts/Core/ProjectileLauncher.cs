using System.Collections;
using Tools.ObjectsPool;
using UnityEngine;

namespace Core
{
	public class ProjectileLauncher : MonoBehaviour
	{
		[SerializeField] private Projectile projectilePrefab;
		[SerializeField] private Transform shootPoint;
		[SerializeField] private float launchPower = 15f;
		[SerializeField, Tooltip("Seconds")] private float shootDelay = 2f;

		private PoolController _poolController;

		private void Start()
		{
			_poolController = PoolController.Instance;
			StartCoroutine(ProjectileLauncherCoroutine());
		}

		private IEnumerator ProjectileLauncherCoroutine()
		{
			while (true)
			{
				Projectile projectile = _poolController.AcquireByPrefabAndSetup(projectilePrefab, shootPoint.position, transform.rotation);
				if (projectile.Rigidbody)
					projectile.Rigidbody.AddForce(projectile.transform.up * launchPower, ForceMode2D.Impulse);
				yield return new WaitForSeconds(shootDelay);
			}
		}
	}
}
