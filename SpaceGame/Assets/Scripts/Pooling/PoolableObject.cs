using System;
using UnityEngine;
using UnityEngine.Events;

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

        [SerializeField] private UnityEvent _onReclaimedByObjectPool;
		public UnityEvent InspectorOnReclaimedByObjectPool => _onReclaimedByObjectPool;

		protected virtual void OnEnable()
		{
            _elapsedTime = 0;
		}

		protected virtual void Update()
        {
            if (ReclaimAfterSeconds < 0)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= ReclaimAfterSeconds)
            {
                OnTimeElapsed();
            }
        }

        protected virtual void OnTimeElapsed()
        {
            ObjectPool.Instance.ReleaseObject(this);
        }

        public void ResetTimer()
        {
            _elapsedTime = 0;
        }
	}
}
