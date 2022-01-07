using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.Ai.Ship
{ 
    public class ShipController : MonoBehaviour, IShipAi
    {
        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private float _forwardSpeed = 30;
        [SerializeField] private float _turnSpeed = 80;

        public Transform Transform => transform;

        public Vector3? Target { get; set; }

        private SequenceNode _rootNode;
        private SelectorNode _findTargetIfNotExists;
        private SequenceNode _findNewTargetIfInRange;
        private SequenceNode _turnAndMoveToTargetSequence;

		private void Awake()
		{
            _findTargetIfNotExists = new SelectorNode(new List<BehaviourNode>()
            {
                new HasTarget(this),
                new FindWanderPoint(this, 100)
            });

            _findNewTargetIfInRange = new SequenceNode(new List<BehaviourNode>()
            {
                new IsWithinRange(this, 10),
                new FindWanderPoint(this, 100)
            });

            _turnAndMoveToTargetSequence = new SequenceNode(new List<BehaviourNode>()
            {
                new TurnToTarget(this),
                new MoveToTarget(this, 10)
            });

            // Get the target and then move to it.
            _rootNode = new SequenceNode(new List<BehaviourNode>()
            {
                _findTargetIfNotExists,
                new SelectorNode(new List<BehaviourNode>()
                { 
                    _findNewTargetIfInRange,
                    _turnAndMoveToTargetSequence
                })
            });
		}

		private void Update()
		{
            _rootNode.Evaluate();
		}

		public void MoveForward(float thrust)
        {
            _rb.velocity = transform.forward * _forwardSpeed * thrust;
        }

        public void TurnTowards(Vector3 heading)
        {
            var desiredRotation = Quaternion.LookRotation(heading);

            var angleDelta = _turnSpeed * Time.deltaTime;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, angleDelta);
        }
    }
}
