using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Weapons.Turret
{ 
    public abstract class Turret : MonoBehaviour
    {
		[SerializeField] protected FiringSystem FiringSystem = default;  // The firing system for turrets should be automatic because of the way they are programmed.

		public virtual TargetLead TargetLead { get; set; }

		[SerializeField] protected Transform Root = default;
		[SerializeField] protected Transform PivotX = default;  // Controls the turret's pitch.
		[SerializeField] protected Transform PivotY = default;  // Controls the turret's yaw.

		[SerializeField] protected Transform TargetFrustumCenter = default;
		[SerializeField] protected float MaxYawDelta = 60F;
		[SerializeField] protected float MaxPitchDelta = 50F;
		[SerializeField] protected float MaxRange = 100F;

		[SerializeField] protected float TurnSpeed = 80F;

		public bool ArePitchYawAndRangeWithinBounds(Vector3 position)
		{
			var localLeadPosition = TargetFrustumCenter.InverseTransformPoint(position);  // This is now the same as the direction from _root (0, 0, 0) to the local position of the target.

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

			float range = (position - TargetFrustumCenter.position).magnitude;
			if (range > MaxRange)
			{
				return false;
			}

			return true;
		}

		[SerializeField] protected float CurrentPitch;
		[SerializeField] protected float CurrentYaw;
		protected void LookAt(TargetLead targetLead)
		{
			//var localLeadPosition = targetLead == null
			//	? Vector3.forward
			//	: TargetFrustumCenter.InverseTransformPoint(targetLead.transform.position);

			//float rotationX = Mathf.Atan2(localLeadPosition.y, localLeadPosition.z) * Mathf.Rad2Deg;
			////float rotationX = Vector3.Angle(Vector3.forward, new Vector3(0, localLeadPosition.y, localLeadPosition.z));
			//float rotationY = Mathf.Atan2(localLeadPosition.x, localLeadPosition.z) * Mathf.Rad2Deg;
			////float rotationY = Vector3.Angle(Vector3.forward, new Vector3(localLeadPosition.x, 0, localLeadPosition.z));

			//var maxAngleDelta = Time.deltaTime * TurnSpeed;
			//var deltaPitch = rotationX - CurrentPitch;
			//var deltaYaw = rotationY - CurrentYaw;

			//if (maxAngleDelta >= Mathf.Abs(deltaPitch))
			//{
			//	CurrentPitch = rotationX;
			//}
			//else
			//{
			//	CurrentPitch += maxAngleDelta * Mathf.Sign(deltaPitch);
			//	CurrentPitch = Mathf.Clamp(CurrentPitch, -MaxPitchDelta, +MaxPitchDelta);
			//}

			//if (maxAngleDelta >= Mathf.Abs(deltaYaw))
			//{
			//	CurrentYaw = rotationY;
			//}
			//else
			//{
			//	CurrentYaw += maxAngleDelta * Mathf.Sign(deltaYaw);
			//	CurrentYaw = Mathf.Clamp(CurrentYaw, -MaxYawDelta, +MaxYawDelta);
			//}

			//PivotX.localRotation = Quaternion.Euler(-CurrentPitch, 0, 0);
			//PivotY.localRotation = Quaternion.Euler(0, CurrentYaw, 0);

			var maxAngleDelta = Time.deltaTime * TurnSpeed;
			if (targetLead != null)
			{
				PivotX.rotation = Quaternion.RotateTowards(PivotX.rotation, Quaternion.LookRotation(targetLead.transform.position - TargetFrustumCenter.position), maxAngleDelta);
			}
			else
			{
				PivotX.rotation = Quaternion.RotateTowards(PivotX.rotation, Quaternion.LookRotation(TargetFrustumCenter.TransformDirection(Vector3.forward)), maxAngleDelta);
			}
		}

		private void Update()
		{
			LookAt(TargetLead);

			//if (TargetLead != null)
			//{ 
			//	Debug.DrawLine(TargetFrustumCenter.position, TargetLead.transform.position, Color.red, 0.1F);
			//}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawFrustum(TargetFrustumCenter.position, MaxPitchDelta * 2, MaxRange, 0, MaxYawDelta / MaxPitchDelta);
		}
	}
}
