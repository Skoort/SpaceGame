using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame
{ 
    public abstract class Turret : MonoBehaviour
    {
		[SerializeField] protected FiringSystem FiringSystem = default;  // The firing system for turrets should be automatic because of the way they are programmed.

		protected virtual TargetLead TargetLead { get; set; }

		[SerializeField] protected Transform Root = default;
		[SerializeField] protected Transform PivotX = default;  // Controls the turret's pitch.
		[SerializeField] protected Transform PivotY = default;  // Controls the turret's yaw.

		[SerializeField] protected Transform TargetFrustumCenter = default;
		[SerializeField] protected float MaxYawDelta = 60F;
		[SerializeField] protected float MaxPitchDelta = 50F;
		[SerializeField] protected float MaxRange = 100F;

		[SerializeField] protected float TurnSpeed = 80F;

		protected bool ArePitchYawAndRangeWithinBounds(Vector3 position)
		{
			var localLeadPosition = Root.InverseTransformPoint(position);  // This is now the same as the direction from _root (0, 0, 0) to the local position of the target.

			float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			if (Mathf.Abs(rotationX) > MaxPitchDelta)
			{
				return false;
			}

			float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;
			if (Mathf.Abs(rotationY) > MaxYawDelta)
			{
				return false;
			}

			float range = localLeadPosition.magnitude;
			if (range > MaxRange)
			{
				return false;
			}

			return true;
		}

		protected float CurrentPitch;
		protected float CurrentYaw;
		protected void LookAt(TargetLead targetLead)
		{
			var localLeadPosition = targetLead == null
				? Vector3.forward
				: Root.InverseTransformPoint(targetLead.transform.position);

			float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;

			var maxAngleDelta = Time.deltaTime * TurnSpeed;
			var deltaPitch = rotationX - CurrentPitch;
			var deltaYaw = rotationY - CurrentYaw;

			if (maxAngleDelta >= Mathf.Abs(deltaPitch))
			{
				CurrentPitch = rotationX;
			}
			else
			{
				CurrentPitch += maxAngleDelta * Mathf.Sign(deltaPitch);
				CurrentPitch = Mathf.Clamp(CurrentPitch, -MaxPitchDelta, +MaxPitchDelta);
			}

			if (maxAngleDelta >= Mathf.Abs(deltaYaw))
			{
				CurrentYaw = rotationY;
			}
			else
			{
				CurrentYaw += maxAngleDelta * Mathf.Sign(deltaYaw);
				CurrentYaw = Mathf.Clamp(CurrentYaw, -MaxYawDelta, +MaxYawDelta);
			}

			PivotX.localRotation = Quaternion.Euler(-CurrentPitch, 0, 0);
			PivotY.localRotation = Quaternion.Euler(0, CurrentYaw, 0);
		}

		private void Update()
		{
			LookAt(TargetLead);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawFrustum(TargetFrustumCenter.position, MaxPitchDelta * 2, MaxRange, 0, MaxYawDelta / MaxPitchDelta);
		}
	}
}
