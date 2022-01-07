using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class HasTarget : BehaviourNode
    {
        private IShipAi _shipAi;

        public HasTarget(IShipAi shipAi)
        {
            _shipAi = shipAi;
        }

		public override NodeState Evaluate()
		{
            if (_shipAi.TargetPosition == null)
            {
                Debug.Log("Has no target!");
                return NodeState.FAILURE;
            }
            else
            {
                Debug.Log("Has a target!");
                return NodeState.SUCCESS;
            }
		}
	}
}
