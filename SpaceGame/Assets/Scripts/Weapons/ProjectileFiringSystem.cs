using SpaceGame.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Weapons
{ 
    public class ProjectileFiringSystem : FiringSystem
	{
		[SerializeField] private GameObject _projectilePrefab = default;

		protected override void DoFire()
		{
			var projectile = ProjectilePool.Instance.RequestProjectile(_projectilePrefab);
			if (Target != null)
			{ 
				// Somehow give the target information to the projectile.
			}
		}
    }
}
