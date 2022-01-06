using UnityEngine;

namespace SpaceGame.Weapons.Targeting
{
	public class Target : MonoBehaviour
    {
		public Team Team;
        [SerializeField] private Rigidbody _rb = default;

		public Vector3 Velocity { get; private set; } = Vector3.zero;

		private void Start()
		{
			GameManager.Instance.RegisterTarget(this);
		}

		private void FixedUpdate()
		{
			if (_rb != null)
			{
				Velocity = _rb.velocity;				
			}
		}

		private void OnDestroy()
		{
			GameManager.Instance.UnregisterTarget(this);
		}

		public Vector3 GetFuturePosition(float time)
		{
			return transform.position + time * Velocity;
		}
	}
}
