using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class MoveToTarget : BehaviourNode
    {
        private IShipAi _shipAi;
        private float _stoppingRange;

        public MoveToTarget(IShipAi shipAi, float stoppingRange)
        {
            _shipAi = shipAi;
            _stoppingRange = stoppingRange;
        }

		public override NodeState Evaluate()
		{
            if (_shipAi.Target == null)
            {
                return NodeState.FAILURE;
            }

            var distanceToTarget = Vector3.Distance(_shipAi.Transform.position, _shipAi.Target.Value);
            if (distanceToTarget <= _stoppingRange)
            {
                return NodeState.SUCCESS;
            }

            var thrust = distanceToTarget / _stoppingRange;
            _shipAi.MoveForward(thrust);

            return NodeState.RUNNING;
		}
	}
}
