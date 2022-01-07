using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai
{
    public enum NodeState
    { 
        SUCCESS,
        RUNNING,
        FAILURE
    }

    [System.Serializable]
    public abstract class BehaviourNode
    {
        public NodeState State { get; protected set; }

        public abstract NodeState Evaluate();
    }
}
