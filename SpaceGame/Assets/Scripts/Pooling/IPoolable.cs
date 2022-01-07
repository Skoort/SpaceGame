using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceGame.Pooling
{ 
    public interface IPoolable
    {
        public float ReclaimAfterSeconds { get; }
        public string ResourceName { get; }
        public GameObject InstanceObject { get; }

        public UnityEvent InspectorOnReclaimedByObjectPool { get; }

        void ResetTimer();
    }
}
