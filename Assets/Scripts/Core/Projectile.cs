using Tools.ObjectsPool;
using UnityEngine;

namespace Core
{
	public class Projectile : PoolableMonoBehaviour
	{
		[Header("Projectile settings")]
		[SerializeField] private Rigidbody2D _rb;

		private float _baseMass, _baseGravityScale;

		public Rigidbody2D Rigidbody => _rb;


		protected override void Start()
		{
			_baseMass = _rb.mass;
			_baseGravityScale = _rb.gravityScale;
			base.Start();
		}

		protected override void OnAcquireAction()
		{
			base.OnAcquireAction();
			_rb.simulated = true;
		}

		protected override void OnReleaseAction()
		{
			base.OnReleaseAction();
			_rb.simulated = false;
			_rb.mass = _baseMass;
			_rb.gravityScale = _baseGravityScale;
			_rb.velocity = Vector2.zero;
			_rb.angularVelocity = 0f;
			_rb.inertia = 0f;
		}
	}
}
