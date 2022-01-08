using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class MoveToTarget : BehaviourNode
    {
        private IShipAi _shipAi;

        public MoveToTarget(IShipAi shipAi)
        {
            _shipAi = shipAi;
        }

		public override NodeState Evaluate()
		{
            if (_shipAi.TargetPosition == null)
            {
                return NodeState.FAILURE;
            }

            var distanceToTarget = Vector3.Distance(_shipAi.Transform.position, _shipAi.TargetPosition.Value);
            if (distanceToTarget <= _shipAi.StoppingRange)
            {
                return NodeState.SUCCESS;
            }

            var thrust = distanceToTarget / (_shipAi.StoppingRange * 0.8F);
            if (thrust > 10)
            {
                thrust = 10;
            }

            _shipAi.MoveForward(thrust);

            return NodeState.RUNNING;
		}
	}
}
