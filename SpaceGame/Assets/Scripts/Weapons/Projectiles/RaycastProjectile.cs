using SpaceGame.Pooling;
using UnityEngine;

namespace SpaceGame.Weapons.Projectiles
{
	public class RaycastProjectile : Projectile
    {
		[SerializeField] private PoolableObject _explosionPrefab = default;

		protected override void Update()
		{
			base.Update();

			var delta = _speed * Time.deltaTime;
			if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, delta, HitLayer.value))
			{
				transform.position = hitInfo.point;
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

			ObjectPool.Instance.RequestObject(_explosionPrefab.ResourceName, _explosionPrefab.InstanceObject, transform);

			base.OnHit();
		}

		public override void OnMiss()
		{
			base.OnHit();
		}
	}
}
