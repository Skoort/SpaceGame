using SpaceGame.Pooling;
using SpaceGame.Weapons.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Weapons
{ 
    public class ProjectileFiringSystem : FiringSystem
	{
		protected override void DoFire()
		{
			foreach (var origin in Origins)
			{ 
				var projectile = ObjectPool.Instance.RequestObject(Weapon.ProjectilePrefab.ResourceName, Weapon.ProjectilePrefab.InstanceObject, origin).GetComponent<Projectile>();
				projectile.Target = Target;
				projectile.HitLayer = HitLayer;
			}
		}
    }
}
