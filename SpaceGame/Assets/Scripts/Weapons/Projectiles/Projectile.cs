using SpaceGame.Pooling;
using UnityEngine;

namespace SpaceGame.Weapons.Projectiles
{
	public abstract class Projectile : PoolableObject
	{
		[SerializeField] protected float _speed = 100;
		public float Speed => _speed;

		public Transform Target { get; set; }
		public LayerMask HitLayer { get; set; }
		public GameObject FiredBy { get; set; }
		public Weapon WeaponInfo { get; set; }

		protected override void OnTimeElapsed()
		{
			OnMiss();

			base.OnTimeElapsed();
		}

		public virtual void OnHit()
		{
			ObjectPool.Instance.ReleaseObject(this);
		}

		public abstract void OnMiss();
	}
}
