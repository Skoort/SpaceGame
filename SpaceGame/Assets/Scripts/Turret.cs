using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using System.Collections;
using UnityEngine;

namespace SpaceGame
{
	public class Turret : MonoBehaviour
    {
        [SerializeField] private FiringSystem _firingSystem = default;
        [SerializeField] private TargetingSystem targetingSystem = default;

        private TargetLead _targetLead;

		[SerializeField] private Transform _root = default;
        [SerializeField] private Transform _pivotX = default;  // Controls the turret's pitch.
        [SerializeField] private Transform _pivotY = default;  // Controls the turret's yaw.

        [SerializeField] private Transform _targetFrustumCenter = default;
        [SerializeField] private float _maxYawDelta   = 60F;
        [SerializeField] private float _maxPitchDelta = 50F;
        [SerializeField] private float _range = 100F;

		[SerializeField] private float _turnSpeed = 80F;


		private IEnumerator FindTarget()
		{
			while (true)
			{
				if (_targetLead != null && ArePitchAndYawWithinBounds(_targetLead.transform.position))
				{
					yield return new WaitForSeconds(0.5F);
				}

				_targetLead = null;  // Set this lead to null and try to find another lead to track.
				foreach (var lead in targetingSystem.Leads)
				{
					if (ArePitchAndYawWithinBounds(lead.transform.position))
					{
						_targetLead = lead;
						break;
					}
				}

				yield return new WaitForSeconds(0.5F);
			}
		}

		private bool ArePitchAndYawWithinBounds(Vector3 position)
		{
			var localLeadPosition = _root.InverseTransformPoint(position);  // This is now the same as the direction from _root (0, 0, 0) to the local position of the target.

			float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;

			return Mathf.Abs(rotationX) <= _maxPitchDelta && Mathf.Abs(rotationY) <= _maxYawDelta;
		}

		private Coroutine _findTargetCoroutine;
		private void Start()
		{
			_findTargetCoroutine = StartCoroutine(FindTarget());
		}

		private void Update()
		{
			if (_targetLead != null)
			{
				_pivotX.LookAt(_targetLead.transform, _root.up);

				// Move to the idle position (pitch 0, yaw 0).
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawFrustum(_targetFrustumCenter.position, _maxPitchDelta * 2, _range, 0, _maxYawDelta / _maxPitchDelta);
		}
	}
}
