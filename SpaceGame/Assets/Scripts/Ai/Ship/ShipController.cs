using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons.Targeting;

namespace SpaceGame.Ai.Ship
{ 
    public class ShipController : MonoBehaviour, IShipAi
    {
        [SerializeField] private TargetingSystem _targetingSystem = default;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private float _forwardSpeed = 30;
        [SerializeField] private float _turnSpeed = 80;

        public Transform Transform => transform;

        private Vector3? _wanderPosition;
        public Vector3? TargetPosition 
        {
            get
            {
                if (CurrentTarget != null)
                {
                    return CurrentTarget.transform.position;
                }
                else
                {
                    return _wanderPosition;
                }
            }
            set
            {
                if (CurrentTarget != null)
                {
                    return;
                }
                _wanderPosition = value;
            } 
        }

        public TargetLead CurrentTarget { get; set; }

        private SequenceNode _rootNode;

        private Team _enemyTeam = Team.HUMANS;
        
		private void Awake()
		{
            var tryToFindLead = new SequenceNode(new List<BehaviourNode>()
            {
                new IsRandomGreaterThan(0.333F),
                new FindNewTarget(this, _targetingSystem)
            });

            var ifNoTargetFindOne = new SelectorNode(new List<BehaviourNode>()
            {
                new HasTarget(this),
                new SelectorNode(new List<BehaviourNode>()
                {
                    tryToFindLead,
                    new FindWanderPoint(this, 100)
                })
            });

            var resetTargetIfInRange = new SequenceNode(new List<BehaviourNode>()
            {
                new IsWithinRange(this, 20),
                new GenericAction(() => { this.CurrentTarget = null; this.TargetPosition = null; return true; })
            });

            var turnAndMoveToTarget = new SequenceNode(new List<BehaviourNode>()
            {
                new TurnToTarget(this),
                new MoveToTarget(this, 10)
            });

            _rootNode = new SequenceNode(new List<BehaviourNode>()
            {
                ifNoTargetFindOne,
                new SelectorNode(new List<BehaviourNode>()
                { 
                    resetTargetIfInRange,
                    turnAndMoveToTarget
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
