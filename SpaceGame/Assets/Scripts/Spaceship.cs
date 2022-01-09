using SpaceGame.Weapons;
using UnityEngine;

namespace SpaceGame
{
	[RequireComponent(typeof(Rigidbody))]
    public class Spaceship : MonoBehaviour
    {
        [Header("*** Ship Movement Settings ***")]
        [SerializeField] private float _yawTorque = 500F;
        [SerializeField] private float _pitchTorque = 1000F;
        [SerializeField] private float _rollTorque = 1000F;
        [SerializeField] private float _thrust = 100F;
        [SerializeField] private float _upThrust = 50F;
        [SerializeField] private float _strafeThrust = 50F;

        [SerializeField] private bool _invertPitch = false;

        [SerializeField] private float _maxVelocity = 100;

        private Rigidbody _rb;

		private float _currentThrust;
        private float _currentStrafe;
        private float _currentUpDown;
        private float _currentRoll;

        [SerializeField] private Camera _camera = default;
        [SerializeField] private LayerMask _mouseCaptureLayer = default;
        [SerializeField] private float _distFromMouseCapturePlane = 100;
        [SerializeField] private Transform _targetLeadMouse = default;
        [SerializeField] private Transform _targetLeadCurrentAim = default;
        
        [SerializeField] private AnimationCurve _turnStrengthCurve = default;

        private FiringSystem _mainGun = default;

		private void Awake()
		{
            _rb = GetComponent<Rigidbody>();

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
		}

		private void FixedUpdate()
		{
            var fixedDeltaTime = Time.fixedDeltaTime;

            CalculateHeading();
            HandleMovement(fixedDeltaTime);
		}

        private void CalculateHeading()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, _distFromMouseCapturePlane, _mouseCaptureLayer.value, QueryTriggerInteraction.Collide))
            {
                _targetLeadMouse.position = hitInfo.point;
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
            }

            if (_mainGun != null)
            { 
                var origin = _mainGun.GetOrigin(0);
                var ray2 = new Ray(origin.position, origin.forward);
                if (Physics.Raycast(ray2, out var hitInfo2, _distFromMouseCapturePlane, _mouseCaptureLayer.value, QueryTriggerInteraction.Collide))
                {
                    _targetLeadMouse.position = hitInfo2.point;
                    Debug.DrawLine(ray.origin, hitInfo2.point, Color.yellow);
                }
            }
        }

        private bool _isHoldingDownMouse0;
        private bool _isHoldingDownMouse1;
		private void HandleMovement(float deltaTime)
		{
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _isHoldingDownMouse0 = true;
            } else
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                _isHoldingDownMouse0 = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                _isHoldingDownMouse1 = true;
            } else
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                _isHoldingDownMouse1 = false;
            }

            if (_isHoldingDownMouse0)
            {
                HandlePitchAndYaw1(deltaTime);
            }
            else
            {
                HandlePitchAndYaw2(deltaTime);
            }

            HandleThrustAndRoll(deltaTime);
        }

        // Pitch and yaw are controlled by moving towards the target (cursor).
        private void HandlePitchAndYaw1(float deltaTime)
        {
            var offset = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2);
            var relOffsetX = offset.x / (Screen.width / 2);
            var relOffsetY = offset.y / (Screen.height / 2);

            var turnStrengthYaw = Mathf.Sign(relOffsetX) *  _turnStrengthCurve.Evaluate(Mathf.Clamp01(Mathf.Abs(relOffsetX)));
            var turnStrengthPitch = Mathf.Sign(relOffsetY) * _turnStrengthCurve.Evaluate(Mathf.Clamp01(Mathf.Abs(relOffsetY)));

            // Pitch
            _rb.AddRelativeTorque(Vector3.right * turnStrengthPitch * -_pitchTorque * deltaTime);

            // Yaw
            _rb.AddRelativeTorque(Vector3.up * turnStrengthYaw * _yawTorque * deltaTime);
        }

        // Pitch and yaw are controlled by movements of the mouse
        private void HandlePitchAndYaw2(float deltaTime)
        {
            var turnStrengthPitch = Input.GetAxis(Strings.RotateX);
            var turnStrengthYaw = Input.GetAxis(Strings.RotateY);

            // Pitch
            _rb.AddRelativeTorque(Vector3.right * turnStrengthPitch * _pitchTorque * (_invertPitch ? +1 : -1) * deltaTime);

            // Yaw
            _rb.AddRelativeTorque(Vector3.up * turnStrengthYaw * _yawTorque * deltaTime);
        }

        private void HandleThrustAndRoll(float deltaTime)
        {
            _currentThrust = Input.GetAxis(Strings.MoveForward);
            _currentUpDown = Input.GetAxis(Strings.MoveVertical);
            _currentStrafe = Input.GetAxis(Strings.MoveHorizontal);
            _currentRoll = Input.GetAxis(Strings.RotateZ);

            // Roll
            _rb.AddRelativeTorque(Vector3.back * _currentRoll * _rollTorque * deltaTime);

            // Thrust
            if (_currentThrust > 0.1F)
            {
                _rb.AddRelativeForce(Vector3.forward * _currentThrust * _thrust * deltaTime);
            } else
            if (_currentThrust < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.forward * _currentThrust * _thrust * 0.5F * deltaTime);
            }

            // Up/down
            if (_currentUpDown > 0.1F || _currentUpDown < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.up * _currentUpDown * _upThrust * deltaTime);
            }

            // Strafing
            if (_currentStrafe > 0.1F || _currentStrafe < -0.1F)
            {
                _rb.AddRelativeForce(Vector3.right * _currentStrafe * _strafeThrust * deltaTime);
            }

            if (_rb.velocity.magnitude > _maxVelocity)
            {
                _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);
            }
        }

        public void AssignGun(FiringSystem gun)
        {
            _mainGun = gun;
        }
	}
}
