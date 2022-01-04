using UnityEngine;

namespace SpaceGame
{
	public class LookAt : MonoBehaviour
    {
        [SerializeField] private Transform _target = default;

		[SerializeField, Tooltip("The turn speed in degrees per second. -1 means instantaneous.")] 
		private float _turnSpeed = -1;

		private void FixedUpdate()
		{
			if (_turnSpeed == -1)
			{
				transform.LookAt(_target);
			}
			else
			{
				var desiredRotation = Quaternion.LookRotation(_target.position - transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.fixedDeltaTime * _turnSpeed);
			}
		}
	}
}
