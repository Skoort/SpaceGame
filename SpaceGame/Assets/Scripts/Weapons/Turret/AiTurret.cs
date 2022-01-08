using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using System.Collections;
using UnityEngine;

namespace SpaceGame.Weapons.Turret
{
	public class AiTurret : Turret
    {
        [SerializeField] private TargetingSystem _targetingSystem = default;

		[SerializeField] private TargetLead _manuallyAssignedLead = null;
		public void ManuallyAssignLead(TargetLead lead)
		{
			_manuallyAssignedLead = lead;
			TargetLead = lead;
		}

		[SerializeField] private bool _isTurretActive = true;
		public bool IsTurretActive 
		{ 
			get => _isTurretActive; 
			set
			{
				if (value == false)
				{
					FiringSystem.StopFiring();
				}
				_isTurretActive = value;
			}
		}

		private IEnumerator FindTarget()
		{
			bool shouldWait = false;

			while (true)
			{
				if (shouldWait)
				{
					shouldWait = false;
					yield return new WaitForSeconds(0.1F);
				}

				if (!_isTurretActive)
				{
					FiringSystem.StopFiring();
					shouldWait = true;
					continue;
				}

				if (_manuallyAssignedLead != null)
				{
					TargetLead = _manuallyAssignedLead;
				}

				if (TargetLead != null && ArePitchYawAndRangeWithinBounds(TargetLead.transform.position))
				{
					if (_manuallyAssignedLead != null && !FiringSystem.IsFiring)
					{ 
						FiringSystem.Fire();
					}
					shouldWait = true;
					continue;
				}

				// Set the current lead (if any) to null and stop firing your weapon.
				TargetLead = null;
				FiringSystem.StopFiring();

				if (_manuallyAssignedLead == null)
				{
					// Try to find another lead to track.
					foreach (var lead in _targetingSystem.Leads)  // Because we are accessing TargetingSystem.Leads directly, we don't need to subscribe to any events.
					{
						if (ArePitchYawAndRangeWithinBounds(lead.transform.position))
						{
							TargetLead = lead;
							FiringSystem.Fire();
							break;
						}
					}
				}

				yield return new WaitForSeconds(0.1F);
			}
		}

		private Coroutine _findTargetCoroutine;
		private void Start()
		{
			FiringSystem.OnFinishedReloading += OnReloaded;
			FiringSystem.OnOutOfAmmo += OnOutOfAmmo;
			_targetingSystem.OnTargetLeadRemoved += OnTargetLeadRemoved;

			_findTargetCoroutine = StartCoroutine(FindTarget());
		}

		bool _hasAmmo;
		private void OnReloaded()
		{
			_hasAmmo = true;
			if (TargetLead != null)
			{ 
				FiringSystem.Fire();  // Resume firing if the turret has finished reloading and its target still exists.
			}
		}

		private void OnTargetLeadRemoved(TargetLead lead)
		{
			if (TargetLead == lead)
			{
				TargetLead = null;
				FiringSystem.StopFiring();
			}
		}

		private void OnOutOfAmmo()
		{
			_hasAmmo = false;
			FiringSystem.Reload();
		}

		private void OnDestroy()
		{
			FiringSystem.OnFinishedReloading -= OnReloaded;
			FiringSystem.OnOutOfAmmo -= OnOutOfAmmo;
			_targetingSystem.OnTargetLeadRemoved -= OnTargetLeadRemoved;
			StopCoroutine(_findTargetCoroutine);
		}
	}
}
