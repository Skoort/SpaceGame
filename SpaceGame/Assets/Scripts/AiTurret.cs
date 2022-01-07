using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using System.Collections;
using UnityEngine;

namespace SpaceGame
{
	public class AiTurret : Turret
    {
        [SerializeField] private TargetingSystem _targetingSystem = default;

		private IEnumerator FindTarget()
		{
			while (true)
			{
				if (TargetLead != null && ArePitchYawAndRangeWithinBounds(TargetLead.transform.position))
				{
					yield return new WaitForSeconds(0.1F);
				}

				// Set the current lead (if any) to null and stop firing your weapon.
				TargetLead = null;
				FiringSystem.StopFiring();

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

				yield return new WaitForSeconds(0.1F);
			}
		}

		private Coroutine _findTargetCoroutine;
		private void Start()
		{
			FiringSystem.OnFinishedReloading += OnReloaded;
			FiringSystem.OnOutOfAmmo += OnOutOfAmmo;

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

		private void OnOutOfAmmo()
		{
			_hasAmmo = false;
			FiringSystem.Reload();
		}

		private void OnDestroy()
		{
			FiringSystem.OnFinishedReloading -= OnReloaded;
			FiringSystem.OnOutOfAmmo -= OnOutOfAmmo;
		}
	}
}
