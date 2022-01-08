using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class FindNewWanderPoint : BehaviourNode
    {
        private float _range;
        private IShipAi _shipAi;

        public FindNewWanderPoint(IShipAi shipAi, float range)
        {
            _shipAi = shipAi;
            _range = range;
        }

		public override NodeState Evaluate()
		{
            //Debug.Log("Found wander target!");

            _shipAi.TargetLead = null;
            _shipAi.TargetWanderPoint = _shipAi.Transform.position + Random.insideUnitSphere * _range;
            _shipAi.State = ShipState.GOING_TO_POINT;

            return NodeState.SUCCESS;
		}
	}
}
