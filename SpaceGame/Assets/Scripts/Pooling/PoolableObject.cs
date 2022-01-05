using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Pooling
{ 
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        [SerializeField] private string _resourceName = default;
        public string ResourceName => _resourceName;
        public GameObject InstanceObject => this.gameObject;

        [SerializeField, Tooltip("How long until the ObjectPool reclaims this? -1 means never.")]
        private float _reclaimAfterSeconds = -1;
		public float ReclaimAfterSeconds => _reclaimAfterSeconds;
		private float _elapsedTime;
		public bool IsReclaimed { get; set; }

		private void OnEnable()
		{
            _elapsedTime = 0;
		}

		protected virtual void Update()
        {
            if (ReclaimAfterSeconds == -1)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= ReclaimAfterSeconds)
            {
                OnTimeElapsed();

                ObjectPool.Instance.ReleaseObject(this);
            }
        }

        protected virtual void OnTimeElapsed()
        { 
        }

        public event Action<PoolableObject> OnReclaimed;
	}
}
