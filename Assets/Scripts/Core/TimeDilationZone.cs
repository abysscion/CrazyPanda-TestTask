using UnityEngine;

namespace Core
{
	public class TimeDilationZone : MonoBehaviour
	{
		[Header("Time settings")]
		[SerializeField, Range(0f, 1f)] private float _targetTimeScale;

		public const string ProjectileTag = "Projectile";

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!other.CompareTag(ProjectileTag))
				return;
			Rigidbody2D rb = other.attachedRigidbody;
			if (!rb || !rb.simulated)
				return;

			rb.mass /= _targetTimeScale;
			rb.velocity *= _targetTimeScale;
			rb.angularVelocity *= _targetTimeScale;
			rb.gravityScale *= _targetTimeScale * _targetTimeScale;
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (!other.CompareTag(ProjectileTag))
				return;
			Rigidbody2D rb = other.attachedRigidbody;
			if (!rb || !rb.simulated)
				return;

			rb.mass *= _targetTimeScale;
			rb.velocity /= _targetTimeScale;
			rb.angularVelocity /= _targetTimeScale;
			rb.gravityScale /= _targetTimeScale * _targetTimeScale;
		}

#if UNITY_EDITOR
		private void OnGUI()
		{
			//could be improved to prevent wrong speed applying to objects, that are left time field by caching them in dictionary, where key is projectile and value is time
			//also could be improved to change in slider being applied immidiately to every object in field
			GUI.Box(new Rect(new Vector2(0f, 0f), new Vector2(200, 50)), $"Time scale: {_targetTimeScale}");
			_targetTimeScale = GUI.HorizontalSlider(new Rect(new Vector2(0f, 25f), new Vector2(200, 50)), _targetTimeScale, 0f, 1f);
		}
#endif
	}
}
