using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Weapons;
using SpaceGame.Weapons.Targeting;
using SpaceGame.Weapons.Turret;

namespace SpaceGame.Ai.Ship
{ 
    public class ShipController : MonoBehaviour, IShipAi
    {
        [SerializeField] private TargetingSystem _targetingSystem = default;
        [SerializeField] private AiTurret _aiTurret = default;

        [SerializeField] private Rigidbody _rb = default;

        [SerializeField] private float _forwardSpeed = 30;
        [SerializeField] private float _turnSpeed = 80;

        public Transform Transform => transform;

        public Vector3? TargetPosition 
        {
            get
            {
                if (State == ShipState.PURSUING_TARGET)
                {
                    return TargetLead?.transform?.position;
                } else
                if (State == ShipState.GOING_TO_POINT)
                {
                    return TargetWanderPoint;
                }
                else
                {
                    return null;
                }
            }
        }

        public ShipState State { get; set; }

        private float _stoppingRangeTarget = 30;
        private float _stoppingRangeWanderPoint = 10;
        public float StoppingRange
        {
            get 
            {
                if (State == ShipState.PURSUING_TARGET)
                {
                    return _stoppingRangeTarget;
                } else
                if (State == ShipState.GOING_TO_POINT)
                {
                    return _stoppingRangeWanderPoint;
                }
                else
                {
                    throw new System.Exception($"Invalid access of StoppingRange in state {State}!");
                }
            }
        }

        public Vector3? TargetWanderPoint { get; set; }
        private TargetLead _targetLead;
        public TargetLead TargetLead 
        {
            get => _targetLead;
            set
            {
                _targetLead = value;
                if (value == null)
                {
                    _aiTurret.IsTurretActive = false;
                }
                else
                {
                    _aiTurret.IsTurretActive = true;
                }
                _aiTurret.ManuallyAssignLead(value);
            }
        }

        private SequenceNode _rootNode;

		private void Awake()
		{
            var tryToFindLead = new SequenceNode(new List<BehaviourNode>()
            {
                new IsRandomGreaterThan(50F),
                new FindNewTargetLead(this, _targetingSystem)
            });

            var ifNoTargetFindOne = new SelectorNode(new List<BehaviourNode>()
            {
                new HasTarget(this),
                new SelectorNode(new List<BehaviourNode>()
                {
                    tryToFindLead,
                    new FindNewWanderPoint(this, 100)
                })
            });

            var resetTargetIfInRange = new SequenceNode(new List<BehaviourNode>()
            {
                new IsWithinRange(this),
                new GenericAction(() => { this.State = ShipState.IDLE; this.TargetLead = null; this.TargetWanderPoint = null; return true; })
            });

            var approachTarget = new DoEachNode(new List<BehaviourNode>()
            {
                new TurnToTarget(this),
                new MoveToTarget(this),
            });

            _rootNode = new SequenceNode(new List<BehaviourNode>()
            {
                ifNoTargetFindOne,
                new SelectorNode(new List<BehaviourNode>()
                { 
                    resetTargetIfInRange,
                    approachTarget
                })
            });
		}

        private void OnTargetLeadRemoved(TargetLead lead)
        {
            if (State == ShipState.PURSUING_TARGET && TargetLead == lead)
            {
                TargetLead = null;
                State = ShipState.IDLE;
            }
            else
            {
                Debug.Log($"Illegal state with target lead but not pursuing it!");
            }
        }

		private void Start()
		{
            _targetingSystem.OnTargetLeadRemoved += OnTargetLeadRemoved;
		}

		private void OnDestroy()
		{
            _targetingSystem.OnTargetLeadRemoved -= OnTargetLeadRemoved;
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
