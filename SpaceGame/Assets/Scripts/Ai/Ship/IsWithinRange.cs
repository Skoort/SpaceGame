using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class IsWithinRange : BehaviourNode
    {
        private IShipAi _shipAi;
        private float _range;

        public IsWithinRange(IShipAi shipAi, float range = -1)
        {
            _shipAi = shipAi;
            _range = range;
        }

		public override NodeState Evaluate()
		{
            // By default, uses the ships stopping range.
            var range = _range < 0
                ? _shipAi.StoppingRange
                : _range;

            if (_shipAi.TargetPosition == null || Vector3.Distance(_shipAi.Transform.position, _shipAi.TargetPosition.Value) > range)
            {
                //Debug.Log("Not within range!");
                return NodeState.FAILURE;
            }
            else
            {
                //Debug.Log("Within range!");
                return NodeState.SUCCESS;
            }
		}
	}
}
