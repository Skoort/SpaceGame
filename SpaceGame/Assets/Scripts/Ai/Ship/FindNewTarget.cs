using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Ai.Ship
{
    public class FindNewTarget : BehaviourNode
    {
        private IShipAi _shipAi;
        private TargetingSystem _targetingSystem;

        public FindNewTarget(IShipAi shipAi, TargetingSystem targetingSystem)
        {
            _shipAi = shipAi;
            _targetingSystem = targetingSystem;
        }

        public override NodeState Evaluate()
        {
            if (_targetingSystem.Leads.Count == 0)
            {
                Debug.Log("Failed to find target. There are no leads!");
                return NodeState.FAILURE;
            }
            else
            {
                Debug.Log("Found a lead to follow!");

                var randomChoice = Random.Range(0, _targetingSystem.Leads.Count);

                _shipAi.CurrentTarget = _targetingSystem.Leads[randomChoice];

                return NodeState.SUCCESS;
            }
        }
    }
}
