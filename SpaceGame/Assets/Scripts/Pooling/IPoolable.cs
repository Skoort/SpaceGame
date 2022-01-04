using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Pooling
{ 
    public interface IPoolable
    {
        public float ReclaimAfterSeconds { get; }
        public string ResourceName { get; }
        public GameObject InstanceObject { get; }
    }
}
