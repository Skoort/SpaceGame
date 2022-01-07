using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class TurnToTarget : BehaviourNode
    {
        private IShipAi _shipAi;

        public TurnToTarget(IShipAi shipAi)
        {
            _shipAi = shipAi;
        }

		public override NodeState Evaluate()
		{
            if (_shipAi.Target == null)
            {
                return NodeState.FAILURE;
            }

            var desiredDirection = _shipAi.Target.Value - _shipAi.Transform.position;

            if (Mathf.Abs(Vector3.Angle(_shipAi.Transform.forward, desiredDirection)) < 10)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                _shipAi.TurnTowards(desiredDirection);

                return NodeState.RUNNING;
            }
		}
	}
}
