using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpaceGame.Ai.Ship
{
    public class GenericAction : BehaviourNode
    {
        Func<bool> _behaviour;

        public GenericAction(Func<bool> behaviour)
        {
            _behaviour = behaviour;
        }

        public override NodeState Evaluate()
        {
            if (_behaviour?.Invoke() == true)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}
