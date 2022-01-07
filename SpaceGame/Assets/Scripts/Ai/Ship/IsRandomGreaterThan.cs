using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{
    public class IsRandomGreaterThan : BehaviourNode
    {
        private float _percentChance;

        public IsRandomGreaterThan(float percentChance)
        {
            _percentChance = percentChance;
        }

        public override NodeState Evaluate()
        {
            if (Random.Range(0, 100) > _percentChance)
            {
                Debug.Log("Random is success");
                return NodeState.SUCCESS;
            }
            else
            {
                Debug.Log("Random is failure");
                return NodeState.FAILURE;
            }
        }
    }
}
