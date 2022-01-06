using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using System.Collections;
using UnityEngine;

namespace SpaceGame
{
	public class Turret : MonoBehaviour
    {
        [SerializeField] private FiringSystem _firingSystem = default;  // The firing system for turrets should be automatic because of the way they are programmed.
        [SerializeField] private TargetingSystem _targetingSystem = default;

        private TargetLead _targetLead;

		[SerializeField] private Transform _root = default;
        [SerializeField] private Transform _pivotX = default;  // Controls the turret's pitch.
        [SerializeField] private Transform _pivotY = default;  // Controls the turret's yaw.

        [SerializeField] private Transform _targetFrustumCenter = default;
        [SerializeField] private float _maxYawDelta   = 60F;
        [SerializeField] private float _maxPitchDelta = 50F;
        [SerializeField] private float _maxRange = 100F;

		[SerializeField] private float _turnSpeed = 80F;

		private IEnumerator FindTarget()
		{
			while (true)
			{
				if (_targetLead != null && ArePitchYawAndRangeWithinBounds(_targetLead.transform.position))
				{
					yield return new WaitForSeconds(0.1F);
				}

				// Set the current lead (if any) to null and stop firing your weapon.
				_targetLead = null;
				_firingSystem.StopFiring();

				// Try to find another lead to track.
				foreach (var lead in _targetingSystem.Leads)  // Because we are accessing TargetingSystem.Leads directly, we don't need to subscribe to any events.
				{
					if (ArePitchYawAndRangeWithinBounds(lead.transform.position))
					{
						_targetLead = lead;
						_firingSystem.Fire();
						break;
					}
				}

				yield return new WaitForSeconds(0.1F);
			}
		}

		private bool ArePitchYawAndRangeWithinBounds(Vector3 position)
		{
			var localLeadPosition = _root.InverseTransformPoint(position);  // This is now the same as the direction from _root (0, 0, 0) to the local position of the target.

			float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			if (Mathf.Abs(rotationX) > _maxPitchDelta)
			{
				return false;
			}

			float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;
			if (Mathf.Abs(rotationY) > _maxYawDelta)
			{
				return false;
			}

			float range = localLeadPosition.magnitude;
			if (range > _maxRange)
			{
				return false;
			}

			return true;
		}

		private float _currentPitch;
		private float _currentYaw;
		private void LookAt(TargetLead targetLead)
		{
			var localLeadPosition = targetLead == null
				? Vector3.forward
				: _root.InverseTransformPoint(targetLead.transform.position);

			float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;

			var maxAngleDelta = Time.deltaTime * _turnSpeed;
			var deltaPitch = rotationX - _currentPitch;
			var deltaYaw = rotationY - _currentYaw;

			if (maxAngleDelta >= Mathf.Abs(deltaPitch))
			{
				_currentPitch = rotationX;
			}
			else
			{ 
				_currentPitch += maxAngleDelta * Mathf.Sign(deltaPitch);
				_currentPitch = Mathf.Clamp(_currentPitch, -_maxPitchDelta, +_maxPitchDelta);
			}

			if (maxAngleDelta >= Mathf.Abs(deltaYaw))
			{
				_currentYaw = rotationY;
			}
			else
			{ 
				_currentYaw += maxAngleDelta * Mathf.Sign(deltaYaw);
				_currentYaw = Mathf.Clamp(_currentYaw, -_maxYawDelta, +_maxYawDelta);
			}

			_pivotX.localRotation = Quaternion.Euler(-_currentPitch, 0, 0);
			_pivotY.localRotation = Quaternion.Euler(0, _currentYaw, 0);
		}

		private Coroutine _findTargetCoroutine;
		private void Start()
		{
			_firingSystem.OnFinishedReloading += OnReloaded;
			_firingSystem.OnOutOfAmmo += OnOutOfAmmo;

			_findTargetCoroutine = StartCoroutine(FindTarget());
		}

		bool _hasAmmo;
		private void OnReloaded()
		{
			_hasAmmo = true;
			if (_targetLead != null)
			{ 
				_firingSystem.Fire();  // Resume firing if the turret has finished reloading and its target still exists.
			}
		}

		private void OnOutOfAmmo()
		{
			_hasAmmo = false;
			_firingSystem.Reload();
		}

		private void Update()
		{
			LookAt(_targetLead);
		}

		private void OnDestroy()
		{
			_firingSystem.OnFinishedReloading -= OnReloaded;
			_firingSystem.OnOutOfAmmo -= OnOutOfAmmo;
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawFrustum(_targetFrustumCenter.position, _maxPitchDelta * 2, _maxRange, 0, _maxYawDelta / _maxPitchDelta);
		}
	}
}
