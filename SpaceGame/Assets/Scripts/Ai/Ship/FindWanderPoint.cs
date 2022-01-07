using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class FindWanderPoint : BehaviourNode
    {
        private float _range;
        private IShipAi _shipAi;

        public FindWanderPoint(IShipAi shipAi, float range)
        {
            _shipAi = shipAi;
            _range = range;
        }

		public override NodeState Evaluate()
		{
            Debug.Log("Found wander target!");

            _shipAi.CurrentTarget = null;  // When you find a wander point, get rid of the target.
            _shipAi.TargetPosition = _shipAi.Transform.position + Random.insideUnitSphere * _range;

            return NodeState.SUCCESS;
		}
	}
}
