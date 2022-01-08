using SpaceGame.Pooling;
using UnityEngine;

namespace SpaceGame.Weapons.Projectiles
{
	public class RaycastProjectile : Projectile
    {
		[SerializeField] private PoolableObject _explosionPrefab = default;

		private void FixedUpdate()
		{
			var delta = _speed * Time.fixedDeltaTime;
			if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, delta, HitLayer.value) && this.FiredBy != hitInfo.transform.root.gameObject)
			{
				transform.position = hitInfo.point;

				var hullInfo = hitInfo.transform.root.GetComponent<HullIntegrity>();
				if (hullInfo != null)
				{
					var damage = Random.Range(WeaponInfo.MinDamage, WeaponInfo.MaxDamage);

					hullInfo.TakeDamage(damage, FiredBy);
				}

				OnHit();
			}
			else
			{ 
				transform.position += transform.forward * delta;
			}
		}

		public override void OnHit()
		{
			Debug.Log($"{gameObject.name} hit something!");

			ObjectPool.Instance.RequestObject(
				_explosionPrefab.ResourceName, 
				_explosionPrefab.gameObject, 
				transform.position, 
				transform.rotation);

			base.OnHit();
		}

		public override void OnMiss()
		{
		}
	}
}
