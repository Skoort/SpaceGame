using UnityEngine;

namespace SpaceGame.Weapons
{
	public class HitscanFiringSystem : FiringSystem
	{
		protected override void DoFire()
		{
			foreach (var origin in Origins)
			{
				var fromPosition = origin.position;
				var toPosition = Random.insideUnitSphere * Weapon.Spread + (origin.position + origin.forward * Weapon.Range);
				var direction = (toPosition - fromPosition).normalized;

				if (Physics.Raycast(fromPosition, direction, out var hitInfo, Weapon.Range, HitLayer.value))
				{
					Debug.Log($"Hit {hitInfo.transform.name}!");
					Debug.DrawLine(fromPosition, hitInfo.point, Color.red, 0.1F);
				}
				else
				{
					Debug.DrawLine(fromPosition, toPosition, Color.blue, 0.1F);
				}
			}
		}
	}
}
