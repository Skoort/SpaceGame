using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class IsWithinRange : BehaviourNode
    {
        private IShipAi _shipAi;
        private float _range;

        public IsWithinRange(IShipAi shipAi, float range)
        {
            _shipAi = shipAi;
            _range = range;
        }

		public override NodeState Evaluate()
		{
            if (_shipAi.Target == null || Vector3.Distance(_shipAi.Transform.position, _shipAi.Target.Value) > _range)
            {
                Debug.Log("Not within range!");
                return NodeState.FAILURE;
            }
            else
            {
                Debug.Log("Within range!");
                return NodeState.SUCCESS;
            }
		}
	}
}
