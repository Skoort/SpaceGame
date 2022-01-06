using SpaceGame.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpaceGame.Weapons.Targeting
{
	// Human targetting system: Keeps a marker for every target in the map. There is only one Target, and that is the
	// enemy that is within 25 degrees of the forward axis for X seconds.
	public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] protected LayerMask TargetLayer = default;
        [SerializeField] protected FiringSystem FiringSystem = default;
        [SerializeField] protected TargetLead LeadPrefab = default;

        public List<TargetLead> Leads;

        [SerializeField] protected Transform MeasureDistanceFrom = default;

        public TargetLead GetLead(Target target)
        {
            return Leads.Where(x => x.Target == target).First();
        }

        protected void PositionLeads()
        {
            var projectileSpeed = FiringSystem.Weapon.IsHitscan
                ? float.MaxValue
                : FiringSystem.Weapon.ProjectilePrefab.Speed;
            foreach (var lead in Leads)
            {
                var distanceToLead = (lead.Target.transform.position - MeasureDistanceFrom.position).magnitude;
                var eta = distanceToLead / projectileSpeed;
                lead.transform.position = lead.Target.GetFuturePosition(eta);
            }
        }

        private void Start()
        {
            if (MeasureDistanceFrom == null)
            {
                MeasureDistanceFrom = this.transform;
            }

            Leads = new List<TargetLead>();
            foreach (var target in GameManager.Instance.Targets)
            {
                if (TargetLayer.Contains(target.gameObject.layer))
                {
                    OnTargetAdded(target);  // When you create a new targeting system you must add the targets that already existed.
                }
            }


            GameManager.Instance.OnTargetAdded += OnTargetAdded;
            GameManager.Instance.OnTargetRemoved += OnTargetRemoved;
        }

        protected void LateUpdate()
        {
            PositionLeads();
        }

		private void OnDestroy()
		{
            GameManager.Instance.OnTargetAdded -= OnTargetAdded;
            GameManager.Instance.OnTargetRemoved -= OnTargetRemoved;
        }

        private void OnTargetAdded(Target target)
        {
            if (TargetLayer.Contains(target.gameObject.layer))
            { 
                var lead = ObjectPool.Instance.RequestObject(LeadPrefab.ResourceName, LeadPrefab.gameObject).GetComponent<TargetLead>();
		        lead.Target = target;
                Leads.Add(lead);

                OnTargetLeadAdded?.Invoke(lead);
            }
        }

	    private void OnTargetRemoved(Target target)
        {
            if (TargetLayer.Contains(target.gameObject.layer))
            {
                var lead = Leads.Find(x => x.Target == target);
                Leads.Remove(lead);
                OnTargetLeadRemoved?.Invoke(lead);  // Invoke this behaviour before the lead is reclaimed by the ObjectPool, so that Target is still valid.

                ObjectPool.Instance.ReleaseObject(lead);
            }
        }

        public event Action<TargetLead> OnTargetLeadAdded;
        public event Action<TargetLead> OnTargetLeadRemoved;
    }
}
